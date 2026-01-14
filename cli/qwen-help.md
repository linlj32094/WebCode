# Qwen CLI 帮助文档

## 用法
```
qwen [options]
```

Qwen Code - 启动交互式 CLI，使用 -p/--prompt 进行非交互模式

## 选项

### 模型
- `-m, --model` - 模型名称
  - 默认值: `"qwen3-coder-plus"`

### 提示
- `-p, --prompt` - 提示内容。附加到 stdin 上的输入（如果有）
- `-i, --prompt-interactive` - 执行提供的提示并在交互模式下继续

### 沙箱
- `-s, --sandbox` - 是否在沙箱中运行（布尔值）
- `--sandbox-image` - 沙箱镜像 URI

### 调试和显示
- `-d, --debug` - 是否在调试模式下运行
  - 默认值: `false`
- `--show-memory-usage` - 在状态栏中显示内存使用情况
  - 默认值: `false`
- `--show_memory_usage` - 在状态栏中显示内存使用情况
  - **已弃用**: 使用 `--show-memory-usage` 代替。我们将在未来几周内删除 `--show_memory_usage`
  - 默认值: `false`

### 文件上下文
- `-a, --all-files` - 是否在上下文中包含所有文件
  - 默认值: `false`
- `--all_files` - 是否在上下文中包含所有文件
  - **已弃用**: 使用 `--all-files` 代替。我们将在未来几周内删除 `--all_files`
  - 默认值: `false`

### 自动化
- `-y, --yolo` - 自动接受所有操作（即 YOLO 模式，详见 https://www.youtube.com/watch?v=xvFZjo5PgG0）
  - 默认值: `false`

### 遥测
- `--telemetry` - 是否启用遥测。此标志专门控制是否发送遥测数据。其他 `--telemetry-*` 标志设置特定值，但本身不启用遥测
- `--telemetry-target` - 设置遥测目标（local 或 gcp）。覆盖设置文件
  - 可选值: `"local"`, `"gcp"`
- `--telemetry-otlp-endpoint` - 设置遥测的 OTLP 端点。覆盖环境变量和设置文件
- `--telemetry-log-prompts` - 启用或禁用用户提示的日志记录以用于遥测。覆盖设置文件
- `--telemetry-outfile` - 将所有遥测输出重定向到指定文件

### 检查点
- `-c, --checkpointing` - 启用文件编辑的检查点
  - 默认值: `false`

### 实验性功能
- `--experimental-acp` - 在 ACP 模式下启动代理
- `--allowed-mcp-server-names` - 允许的 MCP 服务器名称（数组）

### 扩展
- `-e, --extensions` - 要使用的扩展列表。如果未提供，则使用所有扩展（数组）
- `-l, --list-extensions` - 列出所有可用扩展并退出

### IDE 模式
- `--ide-mode` - 是否在 IDE 模式下运行

### OpenAI 配置
- `--openai-logging` - 启用 OpenAI API 调用的日志记录以进行调试和分析
- `--openai-api-key` - 用于身份验证的 OpenAI API 密钥
- `--openai-base-url` - OpenAI 基础 URL（用于自定义端点）

### 代理
- `--proxy` - Gemini 客户端的代理，格式为 `schema://user:password@host:port`

### 版本和帮助
- `-v, --version` - 显示版本号
- `-h, --help` - 显示帮助信息

## 使用示例

```bash
# 启动交互式会话
qwen

# 使用提示（非交互模式）
qwen -p "帮我重构这个函数"

# 交互式执行提示后继续
qwen -i "分析这段代码"

# 使用特定模型
qwen -m qwen3-coder-plus

# 启用调试模式
qwen -d

# YOLO 模式（自动接受所有操作）
qwen -y -p "优化这个项目"

# 包含所有文件在上下文中
qwen -a

# 启用沙箱模式
qwen -s --sandbox-image my-sandbox-image

# 显示内存使用情况
qwen --show-memory-usage

# 启用检查点
qwen -c

# 使用自定义 OpenAI 端点
qwen --openai-base-url https://api.custom.com --openai-api-key sk-xxx

# 列出所有可用扩展
qwen -l

# 使用特定扩展
qwen -e extension1 extension2

# 使用代理
qwen --proxy http://user:pass@proxy.com:8080
```
