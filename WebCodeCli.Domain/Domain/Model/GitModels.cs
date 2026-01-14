namespace WebCodeCli.Domain.Domain.Model;

/// <summary>
/// Git 状态信息
/// </summary>
public class GitStatus
{
    /// <summary>
    /// 是否为 Git 仓库
    /// </summary>
    public bool IsGitRepository { get; set; }
    
    /// <summary>
    /// 当前分支名称
    /// </summary>
    public string CurrentBranch { get; set; } = string.Empty;
    
    /// <summary>
    /// 修改的文件列表
    /// </summary>
    public List<string> ModifiedFiles { get; set; } = new();
    
    /// <summary>
    /// 未跟踪的文件列表
    /// </summary>
    public List<string> UntrackedFiles { get; set; } = new();
    
    /// <summary>
    /// 已暂存的文件列表
    /// </summary>
    public List<string> StagedFiles { get; set; } = new();
    
    /// <summary>
    /// 删除的文件列表
    /// </summary>
    public List<string> DeletedFiles { get; set; } = new();
}

/// <summary>
/// Git Diff 结果
/// </summary>
public class GitDiffResult
{
    /// <summary>
    /// 旧版本内容
    /// </summary>
    public string OldContent { get; set; } = string.Empty;
    
    /// <summary>
    /// 新版本内容
    /// </summary>
    public string NewContent { get; set; } = string.Empty;
    
    /// <summary>
    /// 差异行列表
    /// </summary>
    public List<DiffLine> DiffLines { get; set; } = new();
    
    /// <summary>
    /// 添加的行数
    /// </summary>
    public int AddedLines { get; set; }
    
    /// <summary>
    /// 删除的行数
    /// </summary>
    public int DeletedLines { get; set; }
}

/// <summary>
/// 差异行类型
/// </summary>
public enum DiffLineType
{
    /// <summary>
    /// 未改变
    /// </summary>
    Unchanged,
    
    /// <summary>
    /// 添加
    /// </summary>
    Added,
    
    /// <summary>
    /// 删除
    /// </summary>
    Deleted,
    
    /// <summary>
    /// 修改
    /// </summary>
    Modified
}

/// <summary>
/// 差异行
/// </summary>
public class DiffLine
{
    /// <summary>
    /// 旧文件行号（删除或未改变时有值）
    /// </summary>
    public int? OldLineNumber { get; set; }
    
    /// <summary>
    /// 新文件行号（添加或未改变时有值）
    /// </summary>
    public int? NewLineNumber { get; set; }
    
    /// <summary>
    /// 行内容
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    /// 差异类型
    /// </summary>
    public DiffLineType Type { get; set; }
}

