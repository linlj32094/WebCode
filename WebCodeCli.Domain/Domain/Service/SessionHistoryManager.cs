using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using WebCodeCli.Domain.Common.Extensions;
using WebCodeCli.Domain.Domain.Model;
using WebCodeCli.Domain.Domain.Exceptions;

namespace WebCodeCli.Domain.Domain.Service;

/// <summary>
/// 会话历史管理器
/// </summary>
[ServiceDescription(typeof(ISessionHistoryManager), ServiceLifetime.Scoped)]
public class SessionHistoryManager : ISessionHistoryManager, IAsyncDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<SessionHistoryManager> _logger;
    private const string StorageKey = "webcli_sessions"; // 仅用于迁移
    private const int MaxMessagesPerSession = 1000;
    private const int MaxTitleLength = 100;
    private const int SaveDebounceMs = 500;
    
    // 防抖定时器
    private System.Threading.Timer? _saveTimer;
    private readonly object _saveLock = new object();
    private bool _hasPendingSave = false;
    private SessionHistory? _pendingSession = null;
    
    // 会话缓存
    private readonly ConcurrentDictionary<string, SessionHistory> _sessionCache = new();
    private List<SessionHistory>? _allSessionsCache = null;
    private DateTime _cacheTimestamp = DateTime.MinValue;
    private readonly TimeSpan _cacheExpiration = TimeSpan.FromSeconds(10); // 增加缓存时间以提升性能

    public SessionHistoryManager(
        IJSRuntime jsRuntime,
        ILogger<SessionHistoryManager> logger)
    {
        _jsRuntime = jsRuntime;
        _logger = logger;
    }

    /// <summary>
    /// 加载所有会话
    /// </summary>
    public async Task<List<SessionHistory>> LoadSessionsAsync()
    {
        var startTime = DateTime.Now;
        
        try
        {
            // 检查缓存是否有效
            if (_allSessionsCache != null && 
                DateTime.Now - _cacheTimestamp < _cacheExpiration)
            {
                _logger.LogDebug("从缓存加载会话列表，共 {Count} 个会话", _allSessionsCache.Count);
                return _allSessionsCache;
            }

            var sessions = await _jsRuntime.InvokeAsync<List<SessionHistory>>(
                "webCliIndexedDB.loadSessions");
            
            if (sessions == null)
            {
                sessions = new List<SessionHistory>();
            }

            // 更新缓存
            _allSessionsCache = sessions;
            _cacheTimestamp = DateTime.Now;
            
            // 更新会话缓存
            _sessionCache.Clear();
            foreach (var session in sessions)
            {
                _sessionCache[session.SessionId] = session;
            }

            var loadTime = (DateTime.Now - startTime).TotalMilliseconds;
            _logger.LogInformation("成功加载 {Count} 个会话，耗时: {Time:F2}ms", sessions.Count, loadTime);
            
            // 记录性能指标
            try
            {
                await _jsRuntime.InvokeVoidAsync("sessionPerformance.recordSessionOperation", 
                    "load", sessions.Count, loadTime);
            }
            catch
            {
                // 忽略性能记录错误
            }
            
            return sessions;
        }
        catch (JSException ex)
        {
            _logger.LogError(ex, "加载会话列表失败");
            
            // 尝试清空损坏的数据
            try
            {
                await _jsRuntime.InvokeVoidAsync("webCliIndexedDB.clearAllSessions");
                _logger.LogWarning("已清空损坏的会话数据");
            }
            catch (Exception clearEx)
            {
                _logger.LogError(clearEx, "清空损坏数据失败");
            }
            
            return new List<SessionHistory>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "加载会话列表时发生未知错误");
            return new List<SessionHistory>();
        }
    }

    /// <summary>
    /// 保存会话（带防抖）
    /// </summary>
    public Task SaveSessionAsync(SessionHistory session)
    {
        if (session == null)
        {
            throw new ArgumentNullException(nameof(session));
        }

        _logger.LogDebug("收到保存会话请求: {SessionId}, 消息数: {Count}", session.SessionId, session.Messages?.Count ?? 0);

        // 限制消息数量
        TrimMessages(session);

        // 更新时间戳
        session.UpdatedAt = DateTime.Now;

        lock (_saveLock)
        {
            _hasPendingSave = true;
            _pendingSession = session;
            
            _logger.LogDebug("设置防抖定时器，{Ms}ms 后保存", SaveDebounceMs);
            
            // 重置定时器
            _saveTimer?.Dispose();
            _saveTimer = new System.Threading.Timer(async _ =>
            {
                await ExecuteSaveAsync();
            }, null, SaveDebounceMs, Timeout.Infinite);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 执行实际的保存操作
    /// </summary>
    private async Task ExecuteSaveAsync()
    {
        SessionHistory? sessionToSave = null;
        
        lock (_saveLock)
        {
            if (!_hasPendingSave || _pendingSession == null)
            {
                _logger.LogDebug("没有待保存的会话，跳过");
                return;
            }
            
            sessionToSave = _pendingSession;
            _hasPendingSave = false;
            _pendingSession = null;
        }

        if (sessionToSave != null)
        {
            _logger.LogDebug("执行实际保存: {SessionId}", sessionToSave.SessionId);
            await SaveSessionImmediateAsync(sessionToSave);
        }
    }

    /// <summary>
    /// 立即保存会话
    /// </summary>
    public async Task SaveSessionImmediateAsync(SessionHistory session)
    {
        if (session == null)
        {
            throw new ArgumentNullException(nameof(session));
        }

        var startTime = DateTime.Now;

        try
        {
            // 限制消息数量
            TrimMessages(session);

            // 更新时间戳
            session.UpdatedAt = DateTime.Now;

            // 直接保存单个会话到 IndexedDB（无需加载所有会话）
            var success = await _jsRuntime.InvokeAsync<bool>(
                "webCliIndexedDB.saveSession", session);

            if (success)
            {
                // 更新缓存
                _sessionCache[session.SessionId] = session;
                
                // 更新全局缓存中的会话
                if (_allSessionsCache != null)
                {
                    var existingIndex = _allSessionsCache.FindIndex(s => s.SessionId == session.SessionId);
                    if (existingIndex >= 0)
                    {
                        _allSessionsCache[existingIndex] = session;
                    }
                    else
                    {
                        _allSessionsCache.Add(session);
                    }
                }
                
                var saveTime = (DateTime.Now - startTime).TotalMilliseconds;
                _logger.LogDebug("会话 {SessionId} 保存成功，耗时: {Time:F2}ms", session.SessionId, saveTime);
                
                // 记录性能指标
                try
                {
                    var sessionCount = _allSessionsCache?.Count ?? 0;
                    await _jsRuntime.InvokeVoidAsync("sessionPerformance.recordSessionOperation", 
                        "save", sessionCount, saveTime);
                }
                catch
                {
                    // 忽略性能记录错误
                }
            }
            else
            {
                _logger.LogWarning("会话 {SessionId} 保存失败", session.SessionId);
                throw new InvalidOperationException("保存会话失败，请稍后重试");
            }
        }
        catch (JSException ex) when (ex.Message.Contains("QuotaExceededError"))
        {
            _logger.LogWarning(ex, "IndexedDB 空间不足，无法保存会话 {SessionId}", session.SessionId);
            
            // 抛出特定的异常类型，便于上层捕获和处理
            throw new QuotaExceededException("存储空间不足，请删除一些旧会话以释放空间", ex);
        }
        catch (JSException ex)
        {
            _logger.LogError(ex, "保存会话 {SessionId} 时发生 JavaScript 错误", session.SessionId);
            throw new InvalidOperationException($"保存会话失败: {ex.Message}", ex);
        }
        catch (QuotaExceededException)
        {
            // 重新抛出 QuotaExceededException
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "保存会话 {SessionId} 失败", session.SessionId);
            throw new InvalidOperationException($"保存会话失败: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// 删除会话
    /// </summary>
    public async Task DeleteSessionAsync(string sessionId)
    {
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            throw new ArgumentException("会话ID不能为空", nameof(sessionId));
        }

        try
        {
            // 直接从 IndexedDB 删除
            var success = await _jsRuntime.InvokeAsync<bool>("webCliIndexedDB.deleteSession", sessionId);

            if (success)
            {
                // 更新缓存
                _sessionCache.TryRemove(sessionId, out _);
                
                // 从全局缓存中移除
                if (_allSessionsCache != null)
                {
                    _allSessionsCache.RemoveAll(s => s.SessionId == sessionId);
                }
                
                _logger.LogInformation("会话 {SessionId} 已删除", sessionId);
            }
            else
            {
                _logger.LogWarning("会话 {SessionId} 不存在或删除失败", sessionId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "删除会话 {SessionId} 失败", sessionId);
            throw;
        }
    }

    /// <summary>
    /// 获取单个会话
    /// </summary>
    public async Task<SessionHistory?> GetSessionAsync(string sessionId)
    {
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            return null;
        }

        try
        {
            // 先检查缓存
            if (_sessionCache.TryGetValue(sessionId, out var cachedSession))
            {
                return cachedSession;
            }

            // 从存储加载
            var sessions = await LoadSessionsAsync();
            var session = sessions.FirstOrDefault(s => s.SessionId == sessionId);

            if (session != null)
            {
                _sessionCache[sessionId] = session;
            }

            return session;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取会话 {SessionId} 失败", sessionId);
            return null;
        }
    }

    /// <summary>
    /// 验证工作区路径
    /// </summary>
    public bool ValidateWorkspacePath(string workspacePath)
    {
        if (string.IsNullOrWhiteSpace(workspacePath))
        {
            return false;
        }

        try
        {
            return Directory.Exists(workspacePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "验证工作区路径失败: {Path}", workspacePath);
            return false;
        }
    }

    /// <summary>
    /// 更新所有会话的工作区有效性状态（不删除失效会话，仅更新状态）
    /// </summary>
    public async Task<int> CleanupInvalidSessionsAsync()
    {
        try
        {
            var sessions = await LoadSessionsAsync();
            var invalidCount = 0;

            foreach (var session in sessions)
            {
                var wasValid = session.IsWorkspaceValid;
                session.IsWorkspaceValid = ValidateWorkspacePath(session.WorkspacePath);

                if (!session.IsWorkspaceValid)
                {
                    invalidCount++;
                    if (wasValid)
                    {
                        // 状态从有效变为失效时记录日志
                        _logger.LogInformation(
                            "工作区失效: {SessionId} - {Title}, 工作区路径: {Path}, 最后更新时间: {UpdatedAt}",
                            session.SessionId,
                            session.Title,
                            session.WorkspacePath,
                            session.UpdatedAt);
                    }
                }
            }

            if (invalidCount > 0)
            {
                _logger.LogInformation("检测到 {Count} 个工作区失效的会话，数据已保留", invalidCount);
            }

            return invalidCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "更新会话工作区状态失败");
            return 0;
        }
    }

    /// <summary>
    /// 生成会话标题
    /// </summary>
    public string GenerateSessionTitle(string firstMessage)
    {
        if (string.IsNullOrWhiteSpace(firstMessage))
        {
            return "新会话";
        }

        // 移除多余的空白字符
        var title = firstMessage.Trim();
        
        // 替换换行符为空格
        title = title.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
        
        // 合并多个空格为一个
        while (title.Contains("  "))
        {
            title = title.Replace("  ", " ");
        }

        // 截断标题
        if (title.Length > 30)
        {
            title = title.Substring(0, 30) + "...";
        }

        return title;
    }

    /// <summary>
    /// 限制消息数量
    /// </summary>
    public void TrimMessages(SessionHistory session)
    {
        if (session == null || session.Messages == null)
        {
            return;
        }

        if (session.Messages.Count > MaxMessagesPerSession)
        {
            var messagesToRemove = session.Messages.Count - MaxMessagesPerSession;
            session.Messages.RemoveRange(0, messagesToRemove);
            
            _logger.LogInformation(
                "会话 {SessionId} 消息数量超限，已删除最早的 {Count} 条消息",
                session.SessionId,
                messagesToRemove);
        }
    }

    /// <summary>
    /// 清除缓存（用于强制刷新）
    /// </summary>
    public void ClearCache()
    {
        _sessionCache.Clear();
        _allSessionsCache = null;
        _cacheTimestamp = DateTime.MinValue;
        _logger.LogDebug("会话缓存已清除");
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        // 确保保存所有待处理的会话
        lock (_saveLock)
        {
            if (_hasPendingSave && _pendingSession != null)
            {
                // 立即保存
                var sessionToSave = _pendingSession;
                _hasPendingSave = false;
                _pendingSession = null;
                
                // 注意：这里不能使用 await，因为在 lock 中
                // 我们将在 finally 中处理
                _ = SaveSessionImmediateAsync(sessionToSave);
            }
        }

        // 停止定时器
        if (_saveTimer != null)
        {
            await _saveTimer.DisposeAsync();
            _saveTimer = null;
        }

        GC.SuppressFinalize(this);
    }
}
