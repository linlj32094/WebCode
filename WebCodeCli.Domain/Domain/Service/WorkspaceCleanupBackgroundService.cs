using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebCodeCli.Domain.Domain.Service;

/// <summary>
/// 后台服务,定期清理过期的会话工作区
/// </summary>
public class WorkspaceCleanupBackgroundService : BackgroundService
{
    private readonly ILogger<WorkspaceCleanupBackgroundService> _logger;
    private readonly ICliExecutorService _cliExecutorService;
    private readonly TimeSpan _cleanupInterval = TimeSpan.FromHours(1); // 每小时清理一次

    public WorkspaceCleanupBackgroundService(
        ILogger<WorkspaceCleanupBackgroundService> logger,
        ICliExecutorService cliExecutorService)
    {
        _logger = logger;
        _cliExecutorService = cliExecutorService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("工作区清理后台服务已启动");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("开始执行工作区清理任务");
                //_cliExecutorService.CleanupExpiredWorkspaces();
                _logger.LogInformation("工作区清理任务完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "工作区清理任务执行失败");
            }

            // 等待下一次清理
            await Task.Delay(_cleanupInterval, stoppingToken);
        }

        _logger.LogInformation("工作区清理后台服务已停止");
    }
}
