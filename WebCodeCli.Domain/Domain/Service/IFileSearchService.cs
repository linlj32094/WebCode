using WebCodeCli.Domain.Domain.Model;

namespace WebCodeCli.Domain.Domain.Service;

/// <summary>
/// 文件搜索服务接口
/// </summary>
public interface IFileSearchService
{
    /// <summary>
    /// 在文件中搜索文本
    /// </summary>
    Task<List<SearchResult>> SearchInFilesAsync(string workspacePath, string searchTerm, SearchOptions options);
    
    /// <summary>
    /// 使用正则表达式搜索
    /// </summary>
    Task<List<SearchResult>> SearchByRegexAsync(string workspacePath, string pattern, SearchOptions options);
    
    /// <summary>
    /// 搜索文件名
    /// </summary>
    Task<List<string>> SearchFileNamesAsync(string workspacePath, string pattern);
}
