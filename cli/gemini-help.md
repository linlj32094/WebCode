# Gemini CLI 帮助文档

## 用法
```
gemini [options]
```

Gemini Code - 启动交互式 CLI

## 选项

### 模型
- `-m, --model` - 模型名称
  - 默认值: `"gemini-2.5-pro"`

### 提示
- `-p, --prompt` - 提示内容。附加到 stdin 上的输入（如果有）

### 沙箱
- `-s, --sandbox` - 是否在沙箱中运行（布尔值）
- `--sandbox-image` - 沙箱镜像 URI

### 调试和显示
- `-d, --debug` - 是否在调试模式下运行
  - 默认值: `false`
- `--show_memory_usage` - 在状态栏中显示内存使用情况
  - 默认值: `false`

### 文件上下文
- `-a, --all_files` - 是否在上下文中包含所有文件
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

### 检查点
- `-c, --checkpointing` - 启用文件编辑的检查点
  - 默认值: `false`

### 版本和帮助
- `-v, --version` - 显示版本号
- `-h, --help` - 显示帮助信息

## 使用示例

```bash
# 启动交互式会话
gemini

# 使用提示
gemini -p "帮我重构这个函数"

# 使用特定模型
gemini -m gemini-2.5-pro

# 启用调试模式
gemini -d

# YOLO 模式（自动接受所有操作）
gemini -y -p "优化这个项目"

# 包含所有文件在上下文中
gemini -a

# 启用沙箱模式
gemini -s --sandbox-image my-sandbox-image

# 显示内存使用情况
gemini --show_memory_usage

# 启用检查点
gemini -c

# 启用遥测到本地
gemini --telemetry --telemetry-target local

# 启用遥测到 GCP
gemini --telemetry --telemetry-target gcp

# 自定义 OTLP 端点
gemini --telemetry --telemetry-otlp-endpoint http://localhost:4318

# 启用提示日志记录
gemini --telemetry --telemetry-log-prompts
```

## 注意事项

- Gemini CLI 与 Qwen CLI 有相似的结构和选项
- 主要区别在于默认模型（gemini-2.5-pro vs qwen3-coder-plus）
- Gemini CLI 不包含 OpenAI 相关配置选项
- Gemini CLI 不包含扩展管理功能
- Gemini CLI 不包含 IDE 模式选项
- Gemini CLI 不包含实验性 ACP 模式
