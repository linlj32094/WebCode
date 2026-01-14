namespace WebCodeCli.Domain.Domain.Model;

/// <summary>
/// Git 提交信息
/// </summary>
public class GitCommit
{
    /// <summary>
    /// 完整的提交哈希
    /// </summary>
    public string Hash { get; set; } = string.Empty;
    
    /// <summary>
    /// 短哈希（前7位）
    /// </summary>
    public string ShortHash { get; set; } = string.Empty;
    
    /// <summary>
    /// 提交作者
    /// </summary>
    public string Author { get; set; } = string.Empty;
    
    /// <summary>
    /// 作者邮箱
    /// </summary>
    public string AuthorEmail { get; set; } = string.Empty;
    
    /// <summary>
    /// 提交日期
    /// </summary>
    public DateTime CommitDate { get; set; }
    
    /// <summary>
    /// 提交信息
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// 父提交哈希列表
    /// </summary>
    public List<string> ParentHashes { get; set; } = new();
}
