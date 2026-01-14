# GitHub Copilot CLI 帮助文档

## 用法
```
copilot [options] [command]
```

GitHub Copilot CLI - AI 驱动的编码助手

## 选项

### 目录和路径
- `--add-dir <directory>` - 将目录添加到文件访问的允许列表（可多次使用）
- `--allow-all-paths` - 禁用文件路径验证并允许访问任何路径

### MCP 配置
- `--additional-mcp-config <json>` - 额外的 MCP 服务器配置，作为 JSON 字符串或文件路径（前缀 @）
  - 可多次使用
  - 增强此会话的 `~/.copilot/mcp-config.json` 配置
- `--disable-builtin-mcps` - 禁用所有内置 MCP 服务器（当前：github-mcp-server）
- `--disable-mcp-server <server-name>` - 禁用特定 MCP 服务器（可多次使用）

### 工具权限
- `--allow-all-tools` - 允许所有工具自动运行而无需确认
  - 非交互模式必需
  - 环境变量: `COPILOT_ALLOW_ALL`
- `--allow-tool [tools...]` - 允许特定工具
- `--deny-tool [tools...]` - 拒绝特定工具，优先于 `--allow-tool` 或 `--allow-all-tools`
- `--disable-parallel-tools-execution` - 禁用工具的并行执行
  - LLM 仍然可以进行并行工具调用，但它们将按顺序执行

### 会话管理
- `--continue` - 恢复最近的会话
- `--resume [sessionId]` - 从先前的会话恢复（可选择指定会话 ID）

### 模型选择
- `--model <model>` - 设置要使用的 AI 模型
  - 可选值:
    - `claude-sonnet-4.5`
    - `claude-sonnet-4`
    - `claude-haiku-4.5`
    - `gpt-5`

### 提示和执行
- `-p, --prompt <text>` - 直接执行提示而不使用交互模式

### 自定义指令
- `--no-custom-instructions` - 禁用从 AGENTS.md 和相关文件加载自定义指令

### 日志
- `--log-dir <directory>` - 设置日志文件目录
  - 默认值: `~/.copilot/logs/`
- `--log-level <level>` - 设置日志级别
  - 可选值: `none`, `error`, `warning`, `info`, `debug`, `all`, `default`

### 显示选项
- `--banner` - 显示启动横幅
- `--no-color` - 禁用所有颜色输出
- `--screen-reader` - 启用屏幕阅读器优化
- `--stream <mode>` - 启用或禁用流式模式
  - 可选值: `on`, `off`

### 版本和帮助
- `-v, --version` - 显示版本信息
- `-h, --help` - 显示命令帮助

## 命令
- `help [topic]` - 显示帮助信息

## 帮助主题
- `config` - 配置设置
- `commands` - 交互模式命令
- `environment` - 环境变量
- `logging` - 日志记录
- `permissions` - 工具权限

## 使用示例

```bash
# 启动交互模式
copilot

# 非交互模式执行提示
copilot -p "修复 main.js 中的错误" --allow-all-tools

# 使用特定模型启动
copilot --model gpt-5

# 恢复最近的会话
copilot --continue

# 使用会话选择器恢复先前的会话
copilot --resume

# 使用自动批准恢复
copilot --allow-all-tools --resume

# 允许访问额外目录
copilot --add-dir /home/user/projects

# 允许多个目录
copilot --add-dir ~/workspace --add-dir /tmp

# 禁用路径验证（允许访问任何路径）
copilot --allow-all-paths

# 允许所有 git 命令，除了 git push
copilot --allow-tool 'shell(git:*)' --deny-tool 'shell(git push)'

# 允许所有文件编辑
copilot --allow-tool 'write'

# 允许所有工具，但拒绝名为 "MyMCP" 的 MCP 服务器中的一个特定工具
copilot --deny-tool 'MyMCP(denied_tool)' --allow-tool 'MyMCP'

# 使用额外的 MCP 配置
copilot --additional-mcp-config @/path/to/mcp-config.json

# 禁用内置 MCP 服务器
copilot --disable-builtin-mcps

# 查看配置帮助
copilot help config

# 查看权限帮助
copilot help permissions
```
