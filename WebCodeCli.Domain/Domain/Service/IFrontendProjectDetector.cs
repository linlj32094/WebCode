using WebCodeCli.Domain.Domain.Model;

namespace WebCodeCli.Domain.Domain.Service;

/// <summary>
/// 前端项目检测服务接口
/// </summary>
public interface IFrontendProjectDetector
{
    /// <summary>
    /// 检测工作区中的所有前端项目
    /// </summary>
    /// <param name="workspacePath">工作区路径</param>
    /// <returns>检测到的前端项目列表</returns>
    Task<List<FrontendProjectInfo>> DetectProjectsAsync(string workspacePath);

    /// <summary>
    /// 检测指定目录是否为前端项目
    /// </summary>
    /// <param name="projectPath">项目路径</param>
    /// <returns>前端项目信息，如果不是前端项目则返回null</returns>
    Task<FrontendProjectInfo?> DetectSingleProjectAsync(string projectPath);

    /// <summary>
    /// 获取项目类型显示名称
    /// </summary>
    string GetProjectTypeName(FrontendProjectType type);
}

