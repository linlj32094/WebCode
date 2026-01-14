# Codex CLI 帮助文档

## 用法
```
codex [OPTIONS] [PROMPT]
codex [OPTIONS] [PROMPT] <COMMAND>
```

如果未指定子命令，选项将转发到交互式 CLI。

## 参数
- `[PROMPT]` - 可选的用户提示以启动会话

## 选项

### 配置
- `-c, --config <key=value>` - 覆盖从 `~/.codex/config.toml` 加载的配置值
  - 使用点路径（`foo.bar.baz`）覆盖嵌套值
  - `value` 部分解析为 JSON。如果无法解析为 JSON，则将原始字符串用作文字
  - 示例：
    - `-c model="o3"`
    - `-c 'sandbox_permissions=["disk-full-read-access"]'`
    - `-c shell_environment_policy.inherit=all`

- `-p, --profile <CONFIG_PROFILE>` - 从 config.toml 指定默认选项的配置配置文件

### 输入
- `-i, --image <FILE>...` - 附加到初始提示的可选图像

### 模型
- `-m, --model <MODEL>` - 代理应使用的模型
- `--oss` - 选择本地开源模型提供商的便捷标志
  - 等同于 `-c model_provider=oss`
  - 验证本地 Ollama 服务器正在运行

### 沙箱和权限
- `-s, --sandbox <SANDBOX_MODE>` - 执行模型生成的 shell 命令时使用的沙箱策略
  - 可能的值：
    - `read-only` - 只读访问
    - `workspace-write` - 工作区写入权限
    - `danger-full-access` - 完全访问（危险）

- `-a, --ask-for-approval <APPROVAL_POLICY>` - 配置模型在执行命令之前何时需要人工批准
  - 可能的值：
    - `untrusted` - 仅运行"可信"命令（例如 ls、cat、sed）而不要求用户批准。如果模型提出不在"可信"集中的命令，将升级给用户
    - `on-failure` - 运行所有命令而不要求用户批准。仅当命令执行失败时才要求批准，在这种情况下将升级给用户要求非沙箱执行
    - `on-request` - 模型决定何时要求用户批准
    - `never` - 从不要求用户批准。执行失败立即返回给模型

- `--full-auto` - 低摩擦沙箱自动执行的便捷别名（`-a on-failure, --sandbox workspace-write`）

- `--dangerously-bypass-approvals-and-sandbox` - 跳过所有确认提示并在不沙箱的情况下执行命令
  - 极其危险
  - 仅用于在外部沙箱环境中运行

### 工作目录
- `-C, --cd <DIR>` - 告诉代理使用指定目录作为其工作根目录

### 搜索
- `--search` - 启用网络搜索（默认关闭）
  - 启用后，原生 Responses `web_search` 工具可供模型使用（无需每次调用批准）

### 版本和帮助
- `-h, --help` - 打印帮助（使用 '-h' 查看摘要）
- `-V, --version` - 打印版本

## 命令

- `exec` - 非交互式运行 Codex [别名: e]
- `login` - 管理登录
- `logout` - 删除存储的身份验证凭据
- `mcp` - [实验性] 将 Codex 作为 MCP 服务器运行并管理 MCP 服务器
- `mcp-server` - [实验性] 运行 Codex MCP 服务器（stdio 传输）
- `app-server` - [实验性] 运行应用服务器
- `completion` - 生成 shell 补全脚本
- `sandbox` - 在 Codex 提供的沙箱中运行命令 [别名: debug]
- `apply` - 将 Codex 代理生成的最新差异作为 `git apply` 应用到本地工作树 [别名: a]
- `resume` - 恢复先前的交互式会话（默认选择器；使用 --last 继续最近的会话）
- `cloud` - [实验性] 从 Codex Cloud 浏览任务并在本地应用更改
- `help` - 打印此消息或给定子命令的帮助

## 使用示例

```bash
# 启动交互式会话
codex

# 使用提示启动会话
codex "优化这个函数的性能"

# 非交互式执行
codex exec "创建一个新的 React 组件"

# 使用特定模型
codex -m o3 "解释这段代码"

# 完全自动模式（沙箱写入权限）
codex --full-auto "重构项目结构"

# 在特定目录中工作
codex -C /path/to/project "分析代码质量"

# 启用网络搜索
codex --search "查找最佳实践并应用到这个项目"

# 使用开源模型
codex --oss "帮我调试这个错误"

# 应用最新的差异
codex apply

# 恢复上一个会话
codex resume
```
