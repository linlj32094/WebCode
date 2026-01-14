namespace WebCodeCli.Domain.Domain.Model;

/// <summary>
/// 输出结果区域（Tab=输出结果）的持久化状态。
/// </summary>
public class OutputPanelState
{
    public string SessionId { get; set; } = string.Empty;

    /// <summary>
    /// 非 JSONL 模式下的原始输出（会作为 Markdown 渲染）。
    /// JSONL 模式下也可能用于展示最终 assistant 文本。
    /// </summary>
    public string RawOutput { get; set; } = string.Empty;

    /// <summary>
    /// 是否启用 JSONL 流式输出模式。
    /// </summary>
    public bool IsJsonlOutputActive { get; set; }

    /// <summary>
    /// JSONL 模式下的当前 thread id。
    /// </summary>
    public string ActiveThreadId { get; set; } = string.Empty;

    /// <summary>
    /// JSONL 事件列表（用于“命令执行/工具调用”等卡片展示）。
    /// </summary>
    public List<OutputJsonlEvent> JsonlEvents { get; set; } = new();

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public class OutputJsonlEvent
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ItemType { get; set; }
    public OutputJsonlUsageDetail? Usage { get; set; }
    public bool IsUnknown { get; set; }
}

public class OutputJsonlUsageDetail
{
    public long? InputTokens { get; set; }
    public long? CachedInputTokens { get; set; }
    public long? OutputTokens { get; set; }
}
