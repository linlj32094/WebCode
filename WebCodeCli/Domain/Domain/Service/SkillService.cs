using System.Text.RegularExpressions;
using WebCodeCli.Domain.Common.Extensions;
using WebCodeCli.Domain.Domain.Model;
using Microsoft.Extensions.DependencyInjection;

namespace WebCodeCli.Domain.Domain.Service;

public interface ISkillService
{
    Task<List<SkillItem>> GetSkillsAsync();
}

[ServiceDescription(typeof(ISkillService), ServiceLifetime.Scoped)]
public class SkillService : ISkillService
{
    public async Task<List<SkillItem>> GetSkillsAsync()
    {
        var skills = new List<SkillItem>();
        
        // 读取 Claude skills
        var claudeSkillsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".claude", "skills");
        if (Directory.Exists(claudeSkillsPath))
        {
            var claudeSkills = await LoadSkillsFromDirectory(claudeSkillsPath, "claude");
            skills.AddRange(claudeSkills);
        }
        
        // 读取 Codex skills
        var codexSkillsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".codex", "skills");
        if (Directory.Exists(codexSkillsPath))
        {
            var codexSkills = await LoadSkillsFromDirectory(codexSkillsPath, "codex");
            skills.AddRange(codexSkills);
        }
        
        return skills;
    }
    
    private async Task<List<SkillItem>> LoadSkillsFromDirectory(string skillsPath, string source)
    {
        var skills = new List<SkillItem>();
        
        try
        {
            var skillDirectories = Directory.GetDirectories(skillsPath);
            
            foreach (var skillDir in skillDirectories)
            {
                var skillName = Path.GetFileName(skillDir);
                var skillMdPath = Path.Combine(skillDir, "SKILL.md");
                
                if (File.Exists(skillMdPath))
                {
                    var skill = await ParseSkillFile(skillMdPath, source);
                    if (skill != null)
                    {
                        skills.Add(skill);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // 忽略错误，返回空列表
            Console.WriteLine($"Error loading skills from {skillsPath}: {ex.Message}");
        }
        
        return skills;
    }
    
    private async Task<SkillItem?> ParseSkillFile(string filePath, string source)
    {
        try
        {
            var content = await File.ReadAllTextAsync(filePath);
            
            // 解析 front matter
            var frontMatterMatch = Regex.Match(content, @"^---\s*\n(.*?)\n---", RegexOptions.Singleline);
            if (!frontMatterMatch.Success)
            {
                return null;
            }
            
            var frontMatter = frontMatterMatch.Groups[1].Value;
            var nameMatch = Regex.Match(frontMatter, @"name:\s*(.+)");
            var descriptionMatch = Regex.Match(frontMatter, @"description:\s*(.+)");
            
            if (!nameMatch.Success)
            {
                return null;
            }
            
            return new SkillItem
            {
                Name = nameMatch.Groups[1].Value.Trim(),
                Description = descriptionMatch.Success ? descriptionMatch.Groups[1].Value.Trim() : "",
                Source = source
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing skill file {filePath}: {ex.Message}");
            return null;
        }
    }
}
