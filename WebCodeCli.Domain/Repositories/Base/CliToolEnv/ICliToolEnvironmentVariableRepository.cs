using WebCodeCli.Domain.Domain.Model;
using WebCodeCli.Domain.Repositories.Base;

namespace WebCodeCli.Domain.Repositories.Base.CliToolEnv;

/// <summary>
/// CLI 工具环境变量仓储接口
/// </summary>
public interface ICliToolEnvironmentVariableRepository : IRepository<CliToolEnvironmentVariable>
{
    /// <summary>
    /// 获取指定工具的所有环境变量
    /// </summary>
    Task<Dictionary<string, string>> GetEnvironmentVariablesByToolIdAsync(string toolId);

    /// <summary>
    /// 保存工具的环境变量配置
    /// </summary>
    Task<bool> SaveEnvironmentVariablesAsync(string toolId, Dictionary<string, string> envVars);

    /// <summary>
    /// 删除指定工具的所有环境变量
    /// </summary>
    Task<bool> DeleteByToolIdAsync(string toolId);
}
