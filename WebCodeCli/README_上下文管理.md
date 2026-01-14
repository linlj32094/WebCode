# 🎯 智能上下文管理功能

> 让您的 AI 对话更高效，Token 使用更经济！

## 🚀 快速开始

### 1. 打开上下文面板

在 CodeAssistant 页面，点击工具栏的 **"上下文"** 按钮

### 2. 查看统计信息

面板顶部显示：
- 📊 Token 使用量：`65,000 / 100,000`
- 📈 使用率：`65.0%`
- 📝 上下文项：`45 个`

### 3. 管理上下文

- ✅ **勾选/取消勾选**：包含或排除上下文项
- 🔼🔽 **调整优先级**：影响压缩时的保留决策
- 🔍 **搜索筛选**：快速找到需要的内容

### 4. 压缩上下文（可选）

当 Token 使用率过高时：
1. 点击 **"压缩"** 按钮
2. 选择策略（推荐：智能摘要）
3. 点击 **"开始压缩"**

## ✨ 核心功能

| 功能 | 说明 | 效果 |
|------|------|------|
| 🤖 自动提取代码 | 从对话中自动提取代码块 | 独立管理，可单独排除 |
| 🗜️ 智能压缩 | 4 种压缩策略，节省 Token | 节省 30-50% Token |
| 📊 上下文预览 | 可视化管理所有上下文项 | 一目了然，精确控制 |
| 🎯 优先级系统 | 重要内容优先保留 | 确保关键信息不丢失 |
| 🔍 搜索筛选 | 快速定位特定内容 | 提升管理效率 |
| 📤 导出/导入 | 保存和分享上下文 | 团队协作，备份恢复 |

## 📖 文档

- 📘 [完整功能说明](./Docs/上下文管理功能说明.md) - 详细的功能介绍和使用指南
- 🚀 [快速入门](./Docs/上下文管理快速入门.md) - 5 分钟上手教程
- 💻 [示例代码](./Docs/上下文管理示例代码.md) - 12 个实用示例
- 📊 [功能总结](./Docs/上下文管理功能总结.md) - 技术实现和性能指标

## 🎓 使用场景

### 场景 1：长时间对话
**问题**：对话太长，Token 快用完了  
**解决**：使用 "保留最近消息" 压缩，节省 20-40% Token

### 场景 2：代码重构
**问题**：需要 AI 记住多个代码片段  
**解决**：提高代码片段优先级，确保不被压缩

### 场景 3：项目开发
**问题**：需要 AI 了解项目文件  
**解决**：添加工作区文件到上下文

## 💡 最佳实践

1. ⭐ **定期检查** Token 使用率，避免超限
2. 🎯 **设置优先级**，重要内容设为 8-10
3. 🗜️ **及时压缩**，开始新话题时清理旧内容
4. 📤 **导出备份**，重要对话定期保存

## 🔧 技术栈

- **后端**：C# / ASP.NET Core / Blazor Server
- **前端**：Razor Components / Tailwind CSS
- **架构**：服务模式 / 依赖注入 / 响应式设计

## 📊 性能指标

- ⚡ 上下文构建：< 100ms（100 条消息）
- 🗜️ 压缩操作：< 200ms（100 个上下文项）
- 💾 内存使用：约 500KB - 2MB / 会话
- 💰 Token 节省：30-50%（智能压缩）

## 🎯 代码示例

### 基础使用

```csharp
// 1. 构建上下文
await ContextManagerService.BuildContextFromMessagesAsync(sessionId, messages);

// 2. 查看统计
var stats = ContextManagerService.GetContextStatistics(sessionId);
Console.WriteLine($"Token 使用: {stats.UsedTokens} / {stats.TotalTokens}");

// 3. 压缩上下文
var result = await ContextManagerService.CompressContextAsync(
    sessionId, 
    CompressionStrategy.SmartSummary
);
Console.WriteLine($"节省: {result.TokensSaved} tokens");
```

### 高级用法

```csharp
// 添加工作区文件
await ContextManagerService.AddWorkspaceFileToContextAsync(
    sessionId,
    "src/components/Button.tsx"
);

// 搜索上下文
var items = ContextManagerService.SearchContextItems(sessionId, "React");

// 导出上下文
var json = await ContextManagerService.ExportContextAsync(sessionId);
await File.WriteAllTextAsync("backup.json", json);
```

## 🐛 故障排除

### Q: Token 使用率一直很高？
**A**: 手动触发压缩，或排除不重要的历史消息

### Q: AI 忘记了之前的代码？
**A**: 检查代码片段是否被排除，提高其优先级

### Q: 压缩后 AI 回复质量下降？
**A**: 使用 "智能摘要" 策略，增加 `KeepRecentMessages` 配置

## 🤝 贡献

欢迎提交 Issue 和 Pull Request！

## 📄 许可证

MIT License

---

**需要帮助？** 查看 [完整文档](./Docs/上下文管理功能说明.md) 或提交 [Issue](https://github.com/your-repo/issues)

**觉得有用？** 给个 ⭐ Star 支持一下！

