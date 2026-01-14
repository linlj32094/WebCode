using System.Text.RegularExpressions;

namespace WebCodeCli.Domain.Domain.Model;

/// <summary>
/// 上下文项类型
/// </summary>
public enum ContextItemType
{
    /// <summary>用户消息</summary>
    UserMessage,
    /// <summary>助手消息</summary>
    AssistantMessage,
    /// <summary>代码片段</summary>
    CodeSnippet,
    /// <summary>文件引用</summary>
    FileReference,
    /// <summary>工作区文件</summary>
    WorkspaceFile,
    /// <summary>错误信息</summary>
    ErrorMessage
}

/// <summary>
/// 上下文项
/// </summary>
public class ContextItem
{
    /// <summary>唯一标识</summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    /// <summary>类型</summary>
    public ContextItemType Type { get; set; }
    
    /// <summary>内容</summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>原始消息ID（如果来自消息）</summary>
    public string? MessageId { get; set; }
    
    /// <summary>文件路径（如果是文件相关）</summary>
    public string? FilePath { get; set; }
    
    /// <summary>代码语言（如果是代码片段）</summary>
    public string? Language { get; set; }
    
    /// <summary>行号范围（如果是代码片段）</summary>
    public (int Start, int End)? LineRange { get; set; }
    
    /// <summary>Token 估算数量</summary>
    public int EstimatedTokens { get; set; }
    
    /// <summary>是否包含在上下文中</summary>
    public bool IsIncluded { get; set; } = true;
    
    /// <summary>优先级（0-10，越高越重要）</summary>
    public int Priority { get; set; } = 5;
    
    /// <summary>创建时间</summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    /// <summary>标签</summary>
    public List<string> Tags { get; set; } = new();
    
    /// <summary>摘要（用于压缩显示）</summary>
    public string? Summary { get; set; }
}

/// <summary>
/// 上下文管理器配置
/// </summary>
public class ContextManagerConfig
{
    /// <summary>最大上下文 Token 数</summary>
    public int MaxContextTokens { get; set; } = 100000;
    
    /// <summary>自动压缩阈值（达到此比例时触发压缩）</summary>
    public double AutoCompressThreshold { get; set; } = 0.8;
    
    /// <summary>保留最近消息数量</summary>
    public int KeepRecentMessages { get; set; } = 5;
    
    /// <summary>是否自动提取代码片段</summary>
    public bool AutoExtractCodeSnippets { get; set; } = true;
    
    /// <summary>是否自动提取文件引用</summary>
    public bool AutoExtractFileReferences { get; set; } = true;
    
    /// <summary>代码片段最小行数</summary>
    public int MinCodeSnippetLines { get; set; } = 3;
    
    /// <summary>代码片段最大行数</summary>
    public int MaxCodeSnippetLines { get; set; } = 100;
}

/// <summary>
/// 代码片段提取结果
/// </summary>
public class CodeSnippet
{
    public string Language { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? FilePath { get; set; }
    public (int Start, int End)? LineRange { get; set; }
}

/// <summary>
/// 上下文统计信息
/// </summary>
public class ContextStatistics
{
    /// <summary>总 Token 数</summary>
    public int TotalTokens { get; set; }
    
    /// <summary>已使用 Token 数</summary>
    public int UsedTokens { get; set; }
    
    /// <summary>剩余 Token 数</summary>
    public int RemainingTokens => TotalTokens - UsedTokens;
    
    /// <summary>使用率</summary>
    public double UsagePercentage => TotalTokens > 0 ? (double)UsedTokens / TotalTokens * 100 : 0;
    
    /// <summary>上下文项数量</summary>
    public int ItemCount { get; set; }
    
    /// <summary>包含的项数量</summary>
    public int IncludedItemCount { get; set; }
    
    /// <summary>排除的项数量</summary>
    public int ExcludedItemCount { get; set; }
    
    /// <summary>按类型分组的统计</summary>
    public Dictionary<ContextItemType, int> ItemsByType { get; set; } = new();
}

/// <summary>
/// 上下文压缩策略
/// </summary>
public enum CompressionStrategy
{
    /// <summary>保留最近的消息</summary>
    KeepRecent,
    /// <summary>保留高优先级消息</summary>
    KeepHighPriority,
    /// <summary>智能摘要（保留关键信息）</summary>
    SmartSummary,
    /// <summary>移除冗余内容</summary>
    RemoveDuplicates
}

/// <summary>
/// 上下文压缩结果
/// </summary>
public class CompressionResult
{
    /// <summary>压缩前 Token 数</summary>
    public int TokensBefore { get; set; }
    
    /// <summary>压缩后 Token 数</summary>
    public int TokensAfter { get; set; }
    
    /// <summary>节省的 Token 数</summary>
    public int TokensSaved => TokensBefore - TokensAfter;
    
    /// <summary>压缩率</summary>
    public double CompressionRatio => TokensBefore > 0 ? (double)TokensSaved / TokensBefore * 100 : 0;
    
    /// <summary>移除的项</summary>
    public List<ContextItem> RemovedItems { get; set; } = new();
    
    /// <summary>压缩的项</summary>
    public List<ContextItem> CompressedItems { get; set; } = new();
    
    /// <summary>压缩策略</summary>
    public CompressionStrategy Strategy { get; set; }
    
    /// <summary>压缩时间</summary>
    public DateTime CompressedAt { get; set; } = DateTime.Now;
}

/// <summary>
/// 代码片段提取器
/// </summary>
public static class CodeSnippetExtractor
{
    // 常见代码块标记
    private static readonly Regex CodeBlockRegex = new(@"```(\w+)?\s*\n([\s\S]*?)\n```", RegexOptions.Compiled);
    
    // 文件路径模式
    private static readonly Regex FilePathRegex = new(@"(?:^|\s)([a-zA-Z]:[/\\](?:[^/\\:\*\?""<>\|\r\n]+[/\\])*[^/\\:\*\?""<>\|\r\n]+|(?:\.{0,2}/)+[^\s:]+\.\w+)", RegexOptions.Compiled);
    
    // 行号引用模式 (如: file.cs:10-20)
    private static readonly Regex LineReferenceRegex = new(@"([^\s:]+):(\d+)(?:-(\d+))?", RegexOptions.Compiled);
    
    /// <summary>
    /// 从文本中提取代码片段
    /// </summary>
    public static List<CodeSnippet> ExtractCodeSnippets(string text)
    {
        var snippets = new List<CodeSnippet>();
        
        var matches = CodeBlockRegex.Matches(text);
        foreach (Match match in matches)
        {
            var language = match.Groups[1].Value;
            var code = match.Groups[2].Value.Trim();
            
            if (string.IsNullOrWhiteSpace(code))
            {
                continue;
            }
            
            snippets.Add(new CodeSnippet
            {
                Language = string.IsNullOrEmpty(language) ? "text" : language,
                Code = code
            });
        }
        
        return snippets;
    }
    
    /// <summary>
    /// 从文本中提取文件引用
    /// </summary>
    public static List<string> ExtractFileReferences(string text)
    {
        var files = new HashSet<string>();
        
        // 提取文件路径
        var pathMatches = FilePathRegex.Matches(text);
        foreach (Match match in pathMatches)
        {
            var path = match.Groups[1].Value;
            if (IsValidFilePath(path))
            {
                files.Add(path);
            }
        }
        
        // 提取带行号的文件引用
        var lineMatches = LineReferenceRegex.Matches(text);
        foreach (Match match in lineMatches)
        {
            var path = match.Groups[1].Value;
            if (IsValidFilePath(path))
            {
                files.Add(path);
            }
        }
        
        return files.ToList();
    }
    
    /// <summary>
    /// 提取带行号的文件引用
    /// </summary>
    public static List<(string FilePath, int? StartLine, int? EndLine)> ExtractFileReferencesWithLines(string text)
    {
        var references = new List<(string, int?, int?)>();
        
        var matches = LineReferenceRegex.Matches(text);
        foreach (Match match in matches)
        {
            var path = match.Groups[1].Value;
            if (!IsValidFilePath(path))
            {
                continue;
            }
            
            int? startLine = null;
            int? endLine = null;
            
            if (int.TryParse(match.Groups[2].Value, out var start))
            {
                startLine = start;
            }
            
            if (match.Groups[3].Success && int.TryParse(match.Groups[3].Value, out var end))
            {
                endLine = end;
            }
            
            references.Add((path, startLine, endLine));
        }
        
        return references;
    }
    
    /// <summary>
    /// 检测代码语言
    /// </summary>
    public static string DetectLanguage(string code, string? filePath = null)
    {
        if (!string.IsNullOrEmpty(filePath))
        {
            var ext = Path.GetExtension(filePath).ToLower();
            return ext switch
            {
                ".cs" => "csharp",
                ".js" => "javascript",
                ".ts" => "typescript",
                ".py" => "python",
                ".java" => "java",
                ".cpp" or ".cc" or ".cxx" => "cpp",
                ".c" => "c",
                ".go" => "go",
                ".rs" => "rust",
                ".rb" => "ruby",
                ".php" => "php",
                ".swift" => "swift",
                ".kt" => "kotlin",
                ".html" => "html",
                ".css" => "css",
                ".json" => "json",
                ".xml" => "xml",
                ".yaml" or ".yml" => "yaml",
                ".sql" => "sql",
                ".sh" or ".bash" => "bash",
                ".ps1" => "powershell",
                _ => "text"
            };
        }
        
        // 简单的代码特征检测
        if (code.Contains("using System") || code.Contains("namespace "))
            return "csharp";
        if (code.Contains("function ") || code.Contains("const ") || code.Contains("let "))
            return "javascript";
        if (code.Contains("def ") || code.Contains("import "))
            return "python";
        if (code.Contains("public class ") || code.Contains("public static void"))
            return "java";
        
        return "text";
    }
    
    private static bool IsValidFilePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return false;
        }
        
        // 检查是否有文件扩展名
        var ext = Path.GetExtension(path);
        if (string.IsNullOrEmpty(ext))
        {
            return false;
        }
        
        // 排除一些常见的非文件路径
        if (path.StartsWith("http://") || path.StartsWith("https://"))
        {
            return false;
        }
        
        return true;
    }
}

/// <summary>
/// Token 估算器
/// </summary>
public static class TokenEstimator
{
    /// <summary>
    /// 估算文本的 Token 数量（简单估算：1 Token ≈ 4 字符）
    /// </summary>
    public static int EstimateTokens(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return 0;
        }
        
        // 简单估算：英文约 4 字符/Token，中文约 1.5 字符/Token
        var chineseChars = text.Count(c => c >= 0x4E00 && c <= 0x9FFF);
        var otherChars = text.Length - chineseChars;
        
        return (int)Math.Ceiling(chineseChars / 1.5 + otherChars / 4.0);
    }
    
    /// <summary>
    /// 估算代码的 Token 数量
    /// </summary>
    public static int EstimateCodeTokens(string code)
    {
        // 代码通常比普通文本更密集，使用稍微保守的估算
        return (int)Math.Ceiling(code.Length / 3.5);
    }
}

