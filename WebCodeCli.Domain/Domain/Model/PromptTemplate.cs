namespace WebCodeCli.Domain.Domain.Model;

/// <summary>
/// 提示词模板模型
/// </summary>
public class PromptTemplate
{
    /// <summary>
    /// 模板ID
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 模板标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 模板内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 分类（optimization, documentation, debugging, refactoring, testing, review 等）
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// 图标（Emoji 或图标类名）
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// 是否为自定义模板
    /// </summary>
    public bool IsCustom { get; set; }

    /// <summary>
    /// 是否收藏
    /// </summary>
    public bool IsFavorite { get; set; }

    /// <summary>
    /// 模板变量列表（如 ["code", "language", "framework"]）
    /// </summary>
    public List<string> Variables { get; set; } = new();

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

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

