namespace WebCodeCli.Domain.Domain.Model;

/// <summary>
/// 前端项目类型枚举
/// </summary>
public enum FrontendProjectType
{
    Unknown,
    VueVite,
    ReactVite,
    NextJs,
    NuxtJs
}

/// <summary>
/// 前端项目信息
/// </summary>
public class FrontendProjectInfo
{
    /// <summary>
    /// 项目唯一标识（相对路径）
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// 项目名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 项目类型
    /// </summary>
    public FrontendProjectType Type { get; set; }

    /// <summary>
    /// 项目相对路径
    /// </summary>
    public string RelativePath { get; set; } = string.Empty;

    /// <summary>
    /// 项目绝对路径
    /// </summary>
    public string AbsolutePath { get; set; } = string.Empty;

    /// <summary>
    /// 开发服务器启动命令
    /// </summary>
    public string DevCommand { get; set; } = string.Empty;

    /// <summary>
    /// 构建命令
    /// </summary>
    public string BuildCommand { get; set; } = string.Empty;

    /// <summary>
    /// 构建输出目录（相对于项目目录）
    /// </summary>
    public string BuildOutputDir { get; set; } = string.Empty;

    /// <summary>
    /// 是否需要安装依赖
    /// </summary>
    public bool NeedsDependencyInstall { get; set; }

    /// <summary>
    /// 包管理器（npm/yarn/pnpm）
    /// </summary>
    public string PackageManager { get; set; } = "npm";

    /// <summary>
    /// 默认端口（从配置文件读取）
    /// </summary>
    public int? DefaultPort { get; set; }
}

