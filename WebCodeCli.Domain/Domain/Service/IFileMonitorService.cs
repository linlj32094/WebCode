using WebCodeCli.Domain.Domain.Model;

namespace WebCodeCli.Domain.Domain.Service;

/// <summary>
/// 文件监控服务接口
/// </summary>
public interface IFileMonitorService
{
    /// <summary>
    /// 开始监控指定会话的工作区
    /// </summary>
    void StartMonitoring(string sessionId, string workspacePath);
    
    /// <summary>
    /// 停止监控指定会话
    /// </summary>
    void StopMonitoring(string sessionId);
    
    /// <summary>
    /// 获取最近的文件变更事件
    /// </summary>
    List<FileChangeEvent> GetRecentChanges(string sessionId, int count = 50);
    
    /// <summary>
    /// 清除指定会话的变更历史
    /// </summary>
    void ClearHistory(string sessionId);
    
    /// <summary>
    /// 检查指定会话是否正在监控
    /// </summary>
    bool IsMonitoring(string sessionId);
    
    /// <summary>
    /// 文件变更事件
    /// </summary>
    event EventHandler<FileChangeEvent>? OnFileChanged;
}
