# 简单的测试 CLI 工具 - 回显用户输入并添加一些模拟的代码建议

param(
    [Parameter(Mandatory=$true)]
    [string]$prompt
)

# 模拟流式输出
$response = @"
# 代码助手响应

您的问题是: $prompt

## 代码示例

``````csharp
// 这是一个示例代码
public class Example
{
    public void ProcessRequest(string input)
    {
        Console.WriteLine(`$"处理: {input}`");
    }
}
``````

## 建议

1. 确保输入参数不为空
2. 添加异常处理
3. 考虑使用异步方法

希望这能帮到您！
"@

# 逐字符输出，模拟流式效果
$chars = $response.ToCharArray()
foreach ($char in $chars) {
    Write-Host -NoNewline $char
    Start-Sleep -Milliseconds 10
}
Write-Host ""

