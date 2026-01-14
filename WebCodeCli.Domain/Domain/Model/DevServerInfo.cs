namespace WebCodeCli.Domain.Domain.Model;

/// <summary>
/// 开发服务器状态枚举
/// </summary>
public enum DevServerStatus
{
    Stopped,
    Starting,
    Installing,
    Building,
    Running,
    Failed
}

/// <summary>
/// 开发服务器信息
/// </summary>
public class DevServerInfo
{
    /// <summary>
    /// 服务器唯一标识
    /// </summary>
    public string ServerKey { get; set; } = string.Empty;

    /// <summary>
    /// 会话ID
    /// </summary>
    public string SessionId { get; set; } = string.Empty;

    /// <summary>
    /// 项目信息
    /// </summary>
    public FrontendProjectInfo ProjectInfo { get; set; } = new();

    /// <summary>
    /// 服务器状态
    /// </summary>
    public DevServerStatus Status { get; set; }

    /// <summary>
    /// 本地监听端口
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// 进程ID
    /// </summary>
    public int? ProcessId { get; set; }

    /// <summary>
    /// 启动时间
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// 代理URL（供前端访问）
    /// </summary>
    public string ProxyUrl { get; set; } = string.Empty;

    /// <summary>
    /// 本地URL
    /// </summary>
    public string LocalUrl => $"http://localhost:{Port}";

    /// <summary>
    /// 错误信息
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 日志输出（最近的100行）
    /// </summary>
    public List<string> RecentLogs { get; set; } = new();

    /// <summary>
    /// 是否为构建模式
    /// </summary>
    public bool IsBuildMode { get; set; }
}

