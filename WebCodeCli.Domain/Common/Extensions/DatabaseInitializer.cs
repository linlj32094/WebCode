using Microsoft.Extensions.Logging;
using SqlSugar;
using WebCodeCli.Domain.Common.Map;
using WebCodeCli.Domain.Domain.Model;

namespace WebCodeCli.Domain.Common.Extensions;

/// <summary>
/// 数据库表初始化扩展
/// </summary>
public static class DatabaseInitializer
{
    /// <summary>
    /// 初始化CLI工具相关的数据库表
    /// </summary>
    public static void InitializeCliToolTables(this SqlSugarScope db, ILogger? logger = null)
    {
        try
        {
            // 创建CLI工具环境变量表
            logger?.LogInformation("开始初始化 CLI 工具环境变量表...");
            
            db.CodeFirst.InitTables<CliToolEnvironmentVariable>();
            
            logger?.LogInformation("CLI 工具环境变量表初始化成功");
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "初始化 CLI 工具环境变量表失败");
            throw;
        }
    }
}
