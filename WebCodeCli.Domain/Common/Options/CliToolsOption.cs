using WebCodeCli.Domain.Domain.Model;

namespace WebCodeCli.Domain.Common.Options;

/// <summary>
/// CLI 工具配置选项
/// </summary>
public class CliToolsOption
{
    /// <summary>
    /// CLI 工具列表
    /// </summary>
    public List<CliToolConfig> Tools { get; set; } = new();

    /// <summary>
    /// 最大并发执行数
    /// </summary>
    public int MaxConcurrentExecutions { get; set; } = 3;

    /// <summary>
    /// 默认超时时间（秒）
    /// </summary>
    public int DefaultTimeoutSeconds { get; set; } = 300;

    /// <summary>
    /// 是否启用命令白名单验证
    /// </summary>
    public bool EnableCommandWhitelist { get; set; } = true;

    /// <summary>
    /// 临时工作区根目录路径
    /// </summary>
    public string TempWorkspaceRoot { get; set; } = Path.Combine(Path.GetTempPath(), "WebCodeCli", "Workspaces");

    /// <summary>
    /// 会话工作区过期时间(小时),超过此时间未使用的工作区将被清理
    /// </summary>
    public int WorkspaceExpirationHours { get; set; } = 24;

    /// <summary>
    /// npm 全局包安装目录路径,用于CLI工具的基础路径（可选，留空则自动检测）
    /// </summary>
    public string NpmGlobalPath { get; set; } = string.Empty;
}

