namespace WebCodeCli.Domain.Domain.Model;

/// <summary>
/// 快捷操作模型
/// </summary>
public class QuickAction
{
    /// <summary>
    /// 操作ID
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 操作标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 操作内容（要插入的文本）
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 图标（Emoji 或图标类名）
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// 排序顺序
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// 是否为自定义操作
    /// </summary>
    public bool IsCustom { get; set; }

    /// <summary>
    /// 快捷键（可选，如 "Ctrl+1"）
    /// </summary>
    public string Hotkey { get; set; } = string.Empty;

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 使用次数
    /// </summary>
    public int UsageCount { get; set; }
}

