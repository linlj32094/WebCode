using WebCodeCli.Domain.Domain.Model;

namespace WebCodeCli.Domain.Domain.Service;

/// <summary>
/// 上下文管理服务接口
/// </summary>
public interface IContextManagerService
{
    /// <summary>
    /// 从消息列表构建上下文
    /// </summary>
    Task<List<ContextItem>> BuildContextFromMessagesAsync(string sessionId, List<ChatMessage> messages);
    
    /// <summary>
    /// 添加上下文项
    /// </summary>
    void AddContextItem(string sessionId, ContextItem item);
    
    /// <summary>
    /// 移除上下文项
    /// </summary>
    void RemoveContextItem(string sessionId, string itemId);
    
    /// <summary>
    /// 切换上下文项的包含状态
    /// </summary>
    void ToggleContextItem(string sessionId, string itemId);
    
    /// <summary>
    /// 获取会话的所有上下文项
    /// </summary>
    List<ContextItem> GetContextItems(string sessionId);
    
    /// <summary>
    /// 获取会话的上下文统计
    /// </summary>
    ContextStatistics GetContextStatistics(string sessionId);
    
    /// <summary>
    /// 压缩上下文
    /// </summary>
    Task<CompressionResult> CompressContextAsync(string sessionId, CompressionStrategy strategy);
    
    /// <summary>
    /// 自动压缩（当达到阈值时）
    /// </summary>
    Task<CompressionResult?> AutoCompressIfNeededAsync(string sessionId);
    
    /// <summary>
    /// 清空会话上下文
    /// </summary>
    void ClearContext(string sessionId);
    
    /// <summary>
    /// 从工作区文件添加上下文
    /// </summary>
    Task AddWorkspaceFileToContextAsync(string sessionId, string filePath, string? content = null);
    
    /// <summary>
    /// 批量添加工作区文件到上下文
    /// </summary>
    Task AddWorkspaceFilesToContextAsync(string sessionId, List<string> filePaths);
    
    /// <summary>
    /// 搜索上下文项
    /// </summary>
    List<ContextItem> SearchContextItems(string sessionId, string keyword);
    
    /// <summary>
    /// 按类型筛选上下文项
    /// </summary>
    List<ContextItem> FilterContextItemsByType(string sessionId, ContextItemType type);
    
    /// <summary>
    /// 按标签筛选上下文项
    /// </summary>
    List<ContextItem> FilterContextItemsByTag(string sessionId, string tag);
    
    /// <summary>
    /// 更新上下文项优先级
    /// </summary>
    void UpdateContextItemPriority(string sessionId, string itemId, int priority);
    
    /// <summary>
    /// 生成上下文摘要
    /// </summary>
    Task<string> GenerateContextSummaryAsync(string sessionId);
    
    /// <summary>
    /// 导出上下文（用于调试或分享）
    /// </summary>
    Task<string> ExportContextAsync(string sessionId, string format = "json");
    
    /// <summary>
    /// 导入上下文
    /// </summary>
    Task ImportContextAsync(string sessionId, string data, string format = "json");
}

