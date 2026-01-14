using WebCodeCli.Domain.Domain.Model;

namespace WebCodeCli.Domain.Domain.Service;

/// <summary>
/// Git 服务接口
/// </summary>
public interface IGitService
{
    /// <summary>
    /// 检测工作区是否为 Git 仓库
    /// </summary>
    bool IsGitRepository(string workspacePath);
    
    /// <summary>
    /// 获取文件的提交历史
    /// </summary>
    Task<List<GitCommit>> GetFileHistoryAsync(string workspacePath, string filePath, int maxCount = 50);
    
    /// <summary>
    /// 获取特定版本的文件内容
    /// </summary>
    Task<string> GetFileContentAtCommitAsync(string workspacePath, string filePath, string commitHash);
    
    /// <summary>
    /// 获取文件差异
    /// </summary>
    Task<GitDiffResult> GetFileDiffAsync(string workspacePath, string filePath, string fromCommit, string toCommit);
    
    /// <summary>
    /// 获取工作区状态
    /// </summary>
    Task<GitStatus> GetWorkspaceStatusAsync(string workspacePath);
    
    /// <summary>
    /// 获取所有提交历史
    /// </summary>
    Task<List<GitCommit>> GetAllCommitsAsync(string workspacePath, int maxCount = 100);
}
