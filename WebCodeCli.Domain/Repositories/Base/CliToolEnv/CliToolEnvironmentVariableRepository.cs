using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebCodeCli.Domain.Common.Extensions;
using WebCodeCli.Domain.Domain.Model;
using WebCodeCli.Domain.Repositories.Base;
using AntSK.Domain.Repositories.Base;

namespace WebCodeCli.Domain.Repositories.Base.CliToolEnv;

/// <summary>
/// CLI 工具环境变量仓储实现
/// </summary>
[ServiceDescription(typeof(ICliToolEnvironmentVariableRepository), ServiceLifetime.Scoped)]
public class CliToolEnvironmentVariableRepository : Repository<CliToolEnvironmentVariable>, ICliToolEnvironmentVariableRepository
{
    private readonly ILogger<CliToolEnvironmentVariableRepository> _logger;

    public CliToolEnvironmentVariableRepository(ILogger<CliToolEnvironmentVariableRepository> logger)
    {
        _logger = logger;
    }
    /// <summary>
    /// 获取指定工具的所有环境变量
    /// </summary>
    public async Task<Dictionary<string, string>> GetEnvironmentVariablesByToolIdAsync(string toolId)
    {
        try
        {
            var list = await GetListAsync(x => x.ToolId == toolId);
            return list.ToDictionary(x => x.Key, x => x.Value ?? string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取工具 {ToolId} 的环境变量失败", toolId);
            return new Dictionary<string, string>();
        }
    }

    /// <summary>
    /// 保存工具的环境变量配置
    /// </summary>
    public async Task<bool> SaveEnvironmentVariablesAsync(string toolId, Dictionary<string, string> envVars)
    {
        try
        {
            // 删除旧的环境变量
            await DeleteAsync(x => x.ToolId == toolId);

            // 插入新的环境变量
            var entities = envVars.Select(kvp => new CliToolEnvironmentVariable
            {
                ToolId = toolId,
                Key = kvp.Key,
                Value = kvp.Value,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }).ToList();

            if (entities.Any())
            {
                return await InsertRangeAsync(entities);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "保存工具 {ToolId} 的环境变量失败", toolId);
            return false;
        }
    }

    /// <summary>
    /// 删除指定工具的所有环境变量
    /// </summary>
    public async Task<bool> DeleteByToolIdAsync(string toolId)
    {
        try
        {
            return await DeleteAsync(x => x.ToolId == toolId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "删除工具 {ToolId} 的环境变量失败", toolId);
            return false;
        }
    }
}
