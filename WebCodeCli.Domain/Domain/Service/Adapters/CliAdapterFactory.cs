using Microsoft.Extensions.DependencyInjection;
using WebCodeCli.Domain.Common.Extensions;
using WebCodeCli.Domain.Domain.Model;

namespace WebCodeCli.Domain.Domain.Service.Adapters;

/// <summary>
/// CLI工具适配器工厂接口
/// </summary>
public interface ICliAdapterFactory
{
    /// <summary>
    /// 根据工具配置获取对应的适配器
    /// </summary>
    /// <param name="tool">CLI工具配置</param>
    /// <returns>对应的适配器实例，如果没有匹配的适配器则返回null</returns>
    ICliToolAdapter? GetAdapter(CliToolConfig tool);

    /// <summary>
    /// 根据工具ID获取对应的适配器
    /// </summary>
    /// <param name="toolId">工具ID</param>
    /// <returns>对应的适配器实例，如果没有匹配的适配器则返回null</returns>
    ICliToolAdapter? GetAdapter(string toolId);

    /// <summary>
    /// 检查指定工具是否有对应的适配器（支持流式解析）
    /// </summary>
    /// <param name="tool">CLI工具配置</param>
    /// <returns>是否支持流式解析</returns>
    bool SupportsStreamParsing(CliToolConfig tool);

    /// <summary>
    /// 获取所有已注册的适配器
    /// </summary>
    /// <returns>适配器列表</returns>
    IEnumerable<ICliToolAdapter> GetAllAdapters();
}

/// <summary>
/// CLI工具适配器工厂实现
/// 负责管理和分发不同CLI工具的适配器
/// </summary>
[ServiceDescription(typeof(ICliAdapterFactory), ServiceLifetime.Singleton)]
public class CliAdapterFactory : ICliAdapterFactory
{
    private readonly List<ICliToolAdapter> _adapters;

    public CliAdapterFactory()
    {
        // 注册所有内置适配器
        _adapters = new List<ICliToolAdapter>
        {
            new CodexAdapter(),
            new ClaudeCodeAdapter(),
            new OpenCodeAdapter(),
            // 未来可以在这里添加更多适配器
            // new QwenCodeAdapter(),
            // new GeminiAdapter(),
        };
    }

    /// <summary>
    /// 允许通过依赖注入注册自定义适配器
    /// </summary>
    public CliAdapterFactory(IEnumerable<ICliToolAdapter> adapters)
    {
        _adapters = adapters.ToList();
        
        // 确保至少有默认适配器
        if (!_adapters.Any(a => a is CodexAdapter))
        {
            _adapters.Insert(0, new CodexAdapter());
        }
        if (!_adapters.Any(a => a is ClaudeCodeAdapter))
        {
            _adapters.Insert(1, new ClaudeCodeAdapter());
        }
        if (!_adapters.Any(a => a is OpenCodeAdapter))
        {
            _adapters.Insert(2, new OpenCodeAdapter());
        }
    }

    public ICliToolAdapter? GetAdapter(CliToolConfig tool)
    {
        if (tool == null)
            return null;

        foreach (var adapter in _adapters)
        {
            if (adapter.CanHandle(tool))
            {
                return adapter;
            }
        }

        return null;
    }

    public ICliToolAdapter? GetAdapter(string toolId)
    {
        if (string.IsNullOrEmpty(toolId))
            return null;

        foreach (var adapter in _adapters)
        {
            if (adapter.SupportedToolIds.Any(id => 
                string.Equals(id, toolId, StringComparison.OrdinalIgnoreCase)))
            {
                return adapter;
            }
        }

        return null;
    }

    public bool SupportsStreamParsing(CliToolConfig tool)
    {
        var adapter = GetAdapter(tool);
        return adapter?.SupportsStreamParsing ?? false;
    }

    public IEnumerable<ICliToolAdapter> GetAllAdapters()
    {
        return _adapters.AsReadOnly();
    }

    /// <summary>
    /// 注册新的适配器（用于运行时动态添加）
    /// </summary>
    /// <param name="adapter">适配器实例</param>
    public void RegisterAdapter(ICliToolAdapter adapter)
    {
        if (adapter != null && !_adapters.Contains(adapter))
        {
            _adapters.Add(adapter);
        }
    }

    /// <summary>
    /// 移除指定的适配器
    /// </summary>
    /// <param name="adapter">适配器实例</param>
    public void UnregisterAdapter(ICliToolAdapter adapter)
    {
        _adapters.Remove(adapter);
    }
}

