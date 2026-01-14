using SqlSugar;

namespace WebCodeCli.Domain.Domain.Model;

/// <summary>
/// CLI 工具环境变量配置（数据库存储）
/// </summary>
[SugarTable("cli_tool_environment_variables")]
public class CliToolEnvironmentVariable
{
    /// <summary>
    /// 主键ID
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    /// <summary>
    /// CLI 工具ID
    /// </summary>
    [SugarColumn(Length = 50, IsNullable = false, ColumnDescription = "CLI工具ID")]
    public string ToolId { get; set; } = string.Empty;

    /// <summary>
    /// 环境变量键
    /// </summary>
    [SugarColumn(Length = 200, IsNullable = false, ColumnDescription = "环境变量键")]
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// 环境变量值
    /// </summary>
    [SugarColumn(Length = 2000, IsNullable = true, ColumnDescription = "环境变量值")]
    public string? Value { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(IsNullable = false, ColumnDescription = "创建时间")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(IsNullable = false, ColumnDescription = "更新时间")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
