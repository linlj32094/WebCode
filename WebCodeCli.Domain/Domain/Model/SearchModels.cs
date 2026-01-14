namespace WebCodeCli.Domain.Domain.Model;

/// <summary>
/// 搜索选项
/// </summary>
public class SearchOptions
{
    /// <summary>
    /// 是否大小写敏感
    /// </summary>
    public bool CaseSensitive { get; set; } = false;
    
    /// <summary>
    /// 是否全词匹配
    /// </summary>
    public bool WholeWord { get; set; } = false;
    
    /// <summary>
    /// 是否使用正则表达式
    /// </summary>
    public bool UseRegex { get; set; } = false;
    
    /// <summary>
    /// 文件类型过滤（例如：".cs,.razor"）
    /// </summary>
    public string? FileTypeFilter { get; set; }
    
    /// <summary>
    /// 排除的文件夹（例如："bin,obj,node_modules"）
    /// </summary>
    public string? ExcludeFolders { get; set; } = "bin,obj,node_modules,.git,.vs";
    
    /// <summary>
    /// 最大结果数量
    /// </summary>
    public int MaxResults { get; set; } = 1000;
    
    /// <summary>
    /// 上下文行数（显示匹配行前后的行数）
    /// </summary>
    public int ContextLines { get; set; } = 2;
}

/// <summary>
/// 匹配范围
/// </summary>
public class MatchRange
{
    /// <summary>
    /// 起始位置
    /// </summary>
    public int Start { get; set; }
    
    /// <summary>
    /// 结束位置
    /// </summary>
    public int End { get; set; }
    
    /// <summary>
    /// 匹配的文本
    /// </summary>
    public string Text { get; set; } = string.Empty;
}

/// <summary>
/// 搜索结果
/// </summary>
public class SearchResult
{
    /// <summary>
    /// 文件路径（相对于工作区根目录）
    /// </summary>
    public string FilePath { get; set; } = string.Empty;
    
    /// <summary>
    /// 行号（从1开始）
    /// </summary>
    public int LineNumber { get; set; }
    
    /// <summary>
    /// 行内容
    /// </summary>
    public string LineContent { get; set; } = string.Empty;
    
    /// <summary>
    /// 匹配范围列表
    /// </summary>
    public List<MatchRange> Matches { get; set; } = new();
    
    /// <summary>
    /// 上下文行（匹配行前后的行）
    /// </summary>
    public List<ContextLine> ContextLines { get; set; } = new();
}

/// <summary>
/// 上下文行
/// </summary>
public class ContextLine
{
    /// <summary>
    /// 行号
    /// </summary>
    public int LineNumber { get; set; }
    
    /// <summary>
    /// 行内容
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    /// 是否为匹配行
    /// </summary>
    public bool IsMatch { get; set; }
}

