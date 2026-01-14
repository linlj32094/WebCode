namespace WebCodeCli.Domain.Domain.Service.Adapters;

/// <summary>
/// CLI工具统一输出事件模型
/// 用于在不同CLI适配器之间统一事件格式
/// </summary>
public class CliOutputEvent
{
    /// <summary>
    /// 事件类型，如 thread.started, turn.started, item.updated, turn.completed 等
    /// </summary>
    public string EventType { get; set; } = string.Empty;

    /// <summary>
    /// 会话/线程ID，用于会话恢复
    /// </summary>
    public string? SessionId { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 项目类型，如 reasoning, agent_message, command_execution 等
    /// </summary>
    public string? ItemType { get; set; }

    /// <summary>
    /// 是否为错误事件
    /// </summary>
    public bool IsError { get; set; }

    /// <summary>
    /// 错误消息
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 是否为未识别的事件类型
    /// </summary>
    public bool IsUnknown { get; set; }

    /// <summary>
    /// 事件标题（用于UI显示）
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 使用统计信息
    /// </summary>
    public CliOutputEventUsage? Usage { get; set; }

    /// <summary>
    /// 命令执行信息
    /// </summary>
    public CliCommandExecution? CommandExecution { get; set; }

    /// <summary>
    /// 待办列表信息
    /// </summary>
    public List<CliTodoItem>? TodoItems { get; set; }

    /// <summary>
    /// 额外的元数据
    /// </summary>
    public Dictionary<string, object>? Metadata { get; set; }

    /// <summary>
    /// 原始JSON数据（用于调试）
    /// </summary>
    public string? RawJson { get; set; }
}

/// <summary>
/// 使用统计信息
/// </summary>
public class CliOutputEventUsage
{
    public long InputTokens { get; set; }
    public long CachedInputTokens { get; set; }
    public long OutputTokens { get; set; }
}

/// <summary>
/// 命令执行信息
/// </summary>
public class CliCommandExecution
{
    public string? Command { get; set; }
    public string? Output { get; set; }
    public int? ExitCode { get; set; }
    public string? Status { get; set; }
}

/// <summary>
/// 待办事项
/// </summary>
public class CliTodoItem
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Status { get; set; }
}

/// <summary>
/// CLI会话上下文，用于会话恢复
/// </summary>
public class CliSessionContext
{
    /// <summary>
    /// 会话ID（前端生成的）
    /// </summary>
    public string SessionId { get; set; } = string.Empty;

    /// <summary>
    /// CLI工具的线程/会话ID（由CLI工具返回）
    /// </summary>
    public string? CliThreadId { get; set; }

    /// <summary>
    /// 工作目录
    /// </summary>
    public string WorkingDirectory { get; set; } = string.Empty;

    /// <summary>
    /// 是否为恢复会话（后续请求）
    /// </summary>
    public bool IsResume => !string.IsNullOrEmpty(CliThreadId);
}

