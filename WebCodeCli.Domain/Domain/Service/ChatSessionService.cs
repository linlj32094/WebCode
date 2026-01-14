using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using WebCodeCli.Domain.Common.Extensions;
using WebCodeCli.Domain.Domain.Model;

namespace WebCodeCli.Domain.Domain.Service;

/// <summary>
/// 聊天会话服务实现
/// </summary>
[ServiceDescription(typeof(IChatSessionService), ServiceLifetime.Singleton)]
public class ChatSessionService : IChatSessionService
{
    private readonly ConcurrentDictionary<string, ConcurrentBag<ChatMessage>> _sessions = new();

    public void AddMessage(string sessionId, ChatMessage message)
    {
        var messages = _sessions.GetOrAdd(sessionId, _ => new ConcurrentBag<ChatMessage>());
        messages.Add(message);
    }

    public List<ChatMessage> GetMessages(string sessionId)
    {
        if (_sessions.TryGetValue(sessionId, out var messages))
        {
            return messages.OrderBy(m => m.CreatedAt).ToList();
        }
        return new List<ChatMessage>();
    }

    public void ClearSession(string sessionId)
    {
        _sessions.TryRemove(sessionId, out _);
    }

    public void UpdateMessage(string sessionId, string messageId, Action<ChatMessage> updateAction)
    {
        if (_sessions.TryGetValue(sessionId, out var messages))
        {
            var message = messages.FirstOrDefault(m => m.Id == messageId);
            if (message != null)
            {
                updateAction(message);
            }
        }
    }

    public ChatMessage? GetMessage(string sessionId, string messageId)
    {
        if (_sessions.TryGetValue(sessionId, out var messages))
        {
            return messages.FirstOrDefault(m => m.Id == messageId);
        }
        return null;
    }
}

