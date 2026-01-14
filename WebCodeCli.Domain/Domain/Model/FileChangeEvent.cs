namespace WebCodeCli.Domain.Domain.Model;

/// <summary>
/// 文件变更事件
/// </summary>
public class FileChangeEvent
{
    /// <summary>
    /// 文件路径
    /// </summary>
    public string FilePath { get; set; } = string.Empty;
    
    /// <summary>
    /// 变更类型
    /// </summary>
    public FileChangeType ChangeType { get; set; }
    
    /// <summary>
    /// 时间戳
    /// </summary>
    public DateTime Timestamp { get; set; }
    
    /// <summary>
    /// 会话ID
    /// </summary>
    public string SessionId { get; set; } = string.Empty;
    
    /// <summary>
    /// 旧文件路径（用于重命名）
    /// </summary>
    public string? OldFilePath { get; set; }
    
    /// <summary>
    /// 文件大小
    /// </summary>
    public long? FileSize { get; set; }
    
    /// <summary>
    /// 文件扩展名
    /// </summary>
    public string? FileExtension { get; set; }
    
    /// <summary>
    /// 最后修改时间
    /// </summary>
    public DateTime? LastModified { get; set; }
    
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }
    
    /// <summary>
    /// 是否为目录
    /// </summary>
    public bool IsDirectory { get; set; }
    
    /// <summary>
    /// 内容预览（前100字符，仅文本文件）
    /// </summary>
    public string? ContentPreview { get; set; }
}

/// <summary>
/// 文件变更类型
/// </summary>
public enum FileChangeType
{
    /// <summary>
    /// 创建
    /// </summary>
    Created,
    
    /// <summary>
    /// 修改
    /// </summary>
    Modified,
    
    /// <summary>
    /// 删除
    /// </summary>
    Deleted,
    
    /// <summary>
    /// 重命名
    /// </summary>
    Renamed
}
