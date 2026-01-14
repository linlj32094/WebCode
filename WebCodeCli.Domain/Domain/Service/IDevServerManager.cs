using WebCodeCli.Domain.Domain.Model;

namespace WebCodeCli.Domain.Domain.Service;

/// <summary>
/// 开发服务器管理服务接口
/// </summary>
public interface IDevServerManager
{
    /// <summary>
    /// 启动开发服务器
    /// </summary>
    Task<DevServerInfo> StartDevServerAsync(string sessionId, FrontendProjectInfo projectInfo);

    /// <summary>
    /// 启动构建预览服务器
    /// </summary>
    Task<DevServerInfo> StartBuildPreviewAsync(string sessionId, FrontendProjectInfo projectInfo);

    /// <summary>
    /// 停止开发服务器
    /// </summary>
    Task StopDevServerAsync(string sessionId, string serverKey);

    /// <summary>
    /// 获取服务器状态
    /// </summary>
    Task<DevServerInfo?> GetServerInfoAsync(string sessionId, string serverKey);

    /// <summary>
    /// 获取会话的所有服务器
    /// </summary>
    List<DevServerInfo> GetSessionServers(string sessionId);

    /// <summary>
    /// 停止会话的所有服务器
    /// </summary>
    Task StopAllSessionServersAsync(string sessionId);

    /// <summary>
    /// 检查端口是否可用
    /// </summary>
    Task<bool> IsPortAvailableAsync(int port);

    /// <summary>
    /// 分配一个可用端口
    /// </summary>
    Task<int> AllocatePortAsync();
}

