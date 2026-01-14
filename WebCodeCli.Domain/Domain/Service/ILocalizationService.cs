namespace WebCodeCli.Domain.Domain.Service;

/// <summary>
/// 本地化服务接口
/// </summary>
public interface ILocalizationService
{
    /// <summary>
    /// 获取当前语言
    /// </summary>
    Task<string> GetCurrentLanguageAsync();

    /// <summary>
    /// 设置当前语言
    /// </summary>
    Task SetCurrentLanguageAsync(string language);

    /// <summary>
    /// 获取翻译文本
    /// </summary>
    Task<string> GetTranslationAsync(string key, string? language = null);

    /// <summary>
    /// 获取翻译文本（带参数替换）
    /// </summary>
    Task<string> GetTranslationAsync(string key, Dictionary<string, string> parameters, string? language = null);

    /// <summary>
    /// 获取所有翻译
    /// </summary>
    Task<Dictionary<string, object>> GetAllTranslationsAsync(string language);

    /// <summary>
    /// 获取支持的语言列表
    /// </summary>
    List<LanguageInfo> GetSupportedLanguages();

    /// <summary>
    /// 重新加载翻译资源
    /// </summary>
    Task ReloadTranslationsAsync();
}

/// <summary>
/// 语言信息
/// </summary>
public class LanguageInfo
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NativeName { get; set; } = string.Empty;
}

