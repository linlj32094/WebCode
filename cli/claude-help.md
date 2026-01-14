# Claude CLI 帮助文档

## 用法
```
claude [options] [command] [prompt]
```

Claude Code - 默认启动交互式会话，使用 -p/--print 进行非交互式输出

## 参数
- `prompt` - 您的提示内容

## 选项

### 调试和输出
- `-d, --debug [filter]` - 启用调试模式，可选类别过滤（例如 "api,hooks" 或 "!statsig,!file"）
- `--verbose` - 覆盖配置中的详细模式设置
- `-p, --print` - 打印响应并退出（适用于管道）。注意：使用 -p 模式运行时会跳过工作区信任对话框。仅在您信任的目录中使用此标志
- `--output-format <format>` - 输出格式（仅与 --print 一起使用）：
  - `text`（默认）
  - `json`（单个结果）
  - `stream-json`（实时流式传输）
- `--include-partial-messages` - 包含到达的部分消息块（仅与 --print 和 --output-format=stream-json 一起使用）
- `--input-format <format>` - 输入格式（仅与 --print 一起使用）：
  - `text`（默认）
  - `stream-json`（实时流式输入）

### MCP 和工具
- `--mcp-debug` - [已弃用。改用 --debug] 启用 MCP 调试模式（显示 MCP 服务器错误）
- `--mcp-config <configs...>` - 从 JSON 文件或字符串加载 MCP 服务器（空格分隔）
- `--allowedTools, --allowed-tools <tools...>` - 允许的工具名称列表，逗号或空格分隔（例如 "Bash(git:*) Edit"）
- `--disallowedTools, --disallowed-tools <tools...>` - 拒绝的工具名称列表，逗号或空格分隔（例如 "Bash(git:*) Edit"）
- `--strict-mcp-config` - 仅使用 --mcp-config 中的 MCP 服务器，忽略所有其他 MCP 配置

### 权限和安全
- `--dangerously-skip-permissions` - 绕过所有权限检查。仅建议用于无互联网访问的沙箱
- `--permission-mode <mode>` - 会话使用的权限模式
  - `acceptEdits` - 接受编辑
  - `bypassPermissions` - 绕过权限
  - `default` - 默认
  - `plan` - 计划模式

### 系统提示和配置
- `--system-prompt <prompt>` - 会话使用的系统提示
- `--append-system-prompt <prompt>` - 将系统提示附加到默认系统提示
- `--settings <file-or-json>` - 要加载额外设置的 JSON 文件路径或 JSON 字符串
- `--setting-sources <sources>` - 要加载的设置源的逗号分隔列表（user, project, local）

### 会话管理
- `-c, --continue` - 继续最近的对话
- `-r, --resume [sessionId]` - 恢复对话 - 提供会话 ID 或交互式选择要恢复的对话
- `--fork-session` - 恢复时创建新会话 ID 而不是重用原始会话（与 --resume 或 --continue 一起使用）
- `--session-id <uuid>` - 为对话使用特定会话 ID（必须是有效的 UUID）

### 模型选择
- `--model <model>` - 当前会话的模型。提供最新模型的别名（例如 'sonnet' 或 'opus'）或模型的全名（例如 'claude-sonnet-4-5-20250929'）
- `--fallback-model <model>` - 当默认模型过载时启用自动回退到指定模型（仅与 --print 一起使用）

### 其他选项
- `--replay-user-messages` - 将来自 stdin 的用户消息重新发出到 stdout 以进行确认（仅与 --input-format=stream-json 和 --output-format=stream-json 一起使用）
- `--add-dir <directories...>` - 允许工具访问的其他目录
- `--ide` - 如果恰好有一个有效的 IDE 可用，则在启动时自动连接到 IDE
- `--agents <json>` - 定义自定义代理的 JSON 对象（例如 '{"reviewer": {"description": "Reviews code", "prompt": "You are a code reviewer"}}'）
- `-v, --version` - 输出版本号
- `-h, --help` - 显示命令帮助

## 命令

- `mcp` - 配置和管理 MCP 服务器
- `plugin` - 管理 Claude Code 插件
- `migrate-installer` - 从全局 npm 安装迁移到本地安装
- `setup-token` - 设置长期身份验证令牌（需要 Claude 订阅）
- `doctor` - 检查 Claude Code 自动更新程序的健康状况
- `update` - 检查更新并在可用时安装
- `install [options] [target]` - 安装 Claude Code 原生构建。使用 [target] 指定版本（stable、latest 或特定版本）

## 使用示例

```bash
# 启动交互式会话
claude

# 使用提示
claude "帮我重构这个函数"

# 非交互式输出
claude --print "解释这段代码"

# 继续最近的对话
claude --continue

# 使用特定模型
claude --model claude-sonnet-4-5-20250929

# 添加额外的目录访问权限
claude --add-dir /path/to/project

# 使用 JSON 输出格式
claude --print --output-format json "分析这个错误"
```
