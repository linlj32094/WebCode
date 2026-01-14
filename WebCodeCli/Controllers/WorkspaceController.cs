using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using WebCodeCli.Domain.Domain.Service;

namespace WebCodeCli.Controllers;

/// <summary>
/// 工作区静态文件访问控制器
/// 用于支持HTML预览中的CSS/JS等资源相对路径加载
/// </summary>
[ApiController]
[Route("api/workspace")]
public class WorkspaceController : ControllerBase
{
    private readonly ICliExecutorService _cliExecutorService;
    private readonly ILogger<WorkspaceController> _logger;
    private readonly FileExtensionContentTypeProvider _contentTypeProvider;

    public WorkspaceController(
        ICliExecutorService cliExecutorService,
        ILogger<WorkspaceController> logger)
    {
        _cliExecutorService = cliExecutorService;
        _logger = logger;
        _contentTypeProvider = new FileExtensionContentTypeProvider();
    }

    /// <summary>
    /// 获取工作区文件
    /// GET /api/workspace/{sessionId}/files/{**filePath}
    /// 例如: /api/workspace/abc123/files/index.html
    ///      /api/workspace/abc123/files/css/style.css
    /// </summary>
    [HttpGet("{sessionId}/files/{**filePath}")]
    public IActionResult GetFile(string sessionId, string filePath)
    {
        try
        {
            // 获取工作区路径
            var workspacePath = _cliExecutorService.GetSessionWorkspacePath(sessionId);

            if (!Directory.Exists(workspacePath))
            {
                _logger.LogWarning("工作区不存在: SessionId={SessionId}, Path={Path}", sessionId, workspacePath);
                return NotFound(new { error = "工作区不存在" });
            }

            // 解码文件路径（处理URL编码的中文等字符）
            filePath = Uri.UnescapeDataString(filePath);

            // 组合完整路径
            var fullPath = Path.Combine(workspacePath, filePath);

            // 安全检查：确保文件在工作区内（防止目录遍历攻击）
            var normalizedWorkspace = Path.GetFullPath(workspacePath);
            var normalizedFile = Path.GetFullPath(fullPath);

            if (!normalizedFile.StartsWith(normalizedWorkspace, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("检测到路径遍历攻击尝试: SessionId={SessionId}, FilePath={FilePath}", 
                    sessionId, filePath);
                return BadRequest(new { error = "无效的文件路径" });
            }

            // 检查文件是否存在
            if (!System.IO.File.Exists(fullPath))
            {
                _logger.LogDebug("文件不存在: {Path}", fullPath);
                return NotFound(new { error = "文件不存在", path = filePath });
            }

            // 读取文件
            var fileBytes = System.IO.File.ReadAllBytes(fullPath);

            // 确定 Content-Type
            var contentType = "application/octet-stream";
            if (_contentTypeProvider.TryGetContentType(fullPath, out var detectedContentType))
            {
                contentType = detectedContentType;
            }

            _logger.LogDebug("返回文件: {Path}, Size={Size}, ContentType={ContentType}", 
                filePath, fileBytes.Length, contentType);

            // 设置缓存头（可选，提升性能）
            Response.Headers.CacheControl = "public, max-age=300"; // 缓存5分钟

            return File(fileBytes, contentType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取工作区文件失败: SessionId={SessionId}, FilePath={FilePath}", 
                sessionId, filePath);
            return StatusCode(500, new { error = "服务器错误", message = ex.Message });
        }
    }

    /// <summary>
    /// 获取构建输出文件（支持SPA路由fallback）
    /// GET /api/workspace/{sessionId}/build/{projectPath}/{**filePath}
    /// 例如: /api/workspace/abc123/build/my-app/assets/index.js
    /// </summary>
    [HttpGet("{sessionId}/build/{projectPath}/{**filePath}")]
    public IActionResult GetBuildFile(string sessionId, string projectPath, string? filePath = "")
    {
        try
        {
            // 获取工作区路径
            var workspacePath = _cliExecutorService.GetSessionWorkspacePath(sessionId);

            if (!Directory.Exists(workspacePath))
            {
                _logger.LogWarning("工作区不存在: SessionId={SessionId}, Path={Path}", sessionId, workspacePath);
                return NotFound(new { error = "工作区不存在" });
            }

            // 解码路径
            projectPath = Uri.UnescapeDataString(projectPath);
            filePath = string.IsNullOrEmpty(filePath) ? "index.html" : Uri.UnescapeDataString(filePath);

            // 尝试常见的构建输出目录
            var buildDirs = new[] { "dist", "build", ".next/static", ".output/public", "out" };
            string? fullPath = null;

            foreach (var buildDir in buildDirs)
            {
                var tryPath = Path.Combine(workspacePath, projectPath, buildDir, filePath);
                if (System.IO.File.Exists(tryPath))
                {
                    fullPath = tryPath;
                    break;
                }
            }

            // 如果文件不存在，尝试返回 index.html（SPA fallback）
            if (fullPath == null)
            {
                // 查找 index.html
                foreach (var buildDir in buildDirs)
                {
                    var indexPath = Path.Combine(workspacePath, projectPath, buildDir, "index.html");
                    if (System.IO.File.Exists(indexPath))
                    {
                        fullPath = indexPath;
                        _logger.LogDebug("SPA fallback: 返回 index.html for {FilePath}", filePath);
                        break;
                    }
                }
            }

            if (fullPath == null)
            {
                _logger.LogDebug("构建文件不存在: {ProjectPath}/{FilePath}", projectPath, filePath);
                return NotFound(new { error = "文件不存在", path = filePath });
            }

            // 安全检查
            var normalizedWorkspace = Path.GetFullPath(workspacePath);
            var normalizedFile = Path.GetFullPath(fullPath);

            if (!normalizedFile.StartsWith(normalizedWorkspace, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("检测到路径遍历攻击尝试: SessionId={SessionId}, FilePath={FilePath}",
                    sessionId, filePath);
                return BadRequest(new { error = "无效的文件路径" });
            }

            // 读取文件
            var fileBytes = System.IO.File.ReadAllBytes(fullPath);

            // 确定 Content-Type
            var contentType = "application/octet-stream";
            if (_contentTypeProvider.TryGetContentType(fullPath, out var detectedContentType))
            {
                contentType = detectedContentType;
            }

            _logger.LogDebug("返回构建文件: {Path}, Size={Size}, ContentType={ContentType}",
                filePath, fileBytes.Length, contentType);

            // 禁用缓存以便开发时实时更新
            Response.Headers.CacheControl = "no-cache, no-store, must-revalidate";
            Response.Headers.Pragma = "no-cache";
            Response.Headers.Expires = "0";

            return File(fileBytes, contentType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取构建文件失败: SessionId={SessionId}, ProjectPath={ProjectPath}, FilePath={FilePath}",
                sessionId, projectPath, filePath);
            return StatusCode(500, new { error = "服务器错误", message = ex.Message });
        }
    }

    /// <summary>
    /// 健康检查端点
    /// </summary>
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
    }
}

