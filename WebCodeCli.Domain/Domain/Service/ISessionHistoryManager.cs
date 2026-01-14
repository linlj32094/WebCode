using WebCodeCli.Domain.Domain.Model;

namespace WebCodeCli.Domain.Domain.Service;

/// <summary>
/// 会话历史管理器接口
/// </summary>
public interface ISessionHistoryManager
{
    /// <summary>
    /// 加载所有会话
    /// </summary>
    Task<List<SessionHistory>> LoadSessionsAsync();
    
    /// <summary>
    /// 保存会话（带防抖）
    /// </summary>
    Task SaveSessionAsync(SessionHistory session);
    
    /// <summary>
    /// 立即保存会话
    /// </summary>
    Task SaveSessionImmediateAsync(SessionHistory session);
    
    /// <summary>
    /// 删除会话
    /// </summary>
    Task DeleteSessionAsync(string sessionId);
    
    /// <summary>
    /// 获取单个会话
    /// </summary>
    Task<SessionHistory?> GetSessionAsync(string sessionId);
    
    /// <summary>
    /// 验证工作区路径
    /// </summary>
    bool ValidateWorkspacePath(string workspacePath);
    
    /// <summary>
    /// 更新所有会话的工作区有效性状态（不删除失效会话，仅更新状态）
    /// </summary>
    Task<int> CleanupInvalidSessionsAsync();
    
    /// <summary>
    /// 生成会话标题
    /// </summary>
    string GenerateSessionTitle(string firstMessage);
    
    /// <summary>
    /// 限制消息数量
    /// </summary>
    void TrimMessages(SessionHistory session);
    
    /// <summary>
    /// 清除缓存（用于强制刷新）
    /// </summary>
    void ClearCache();
}
