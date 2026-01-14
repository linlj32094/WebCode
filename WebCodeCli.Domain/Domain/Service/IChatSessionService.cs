using WebCodeCli.Domain.Domain.Model;

namespace WebCodeCli.Domain.Domain.Service;

/// <summary>
/// 聊天会话服务接口
/// </summary>
public interface IChatSessionService
{
    /// <summary>
    /// 添加消息到会话
    /// </summary>
    /// <param name="sessionId">会话ID</param>
    /// <param name="message">消息</param>
    void AddMessage(string sessionId, ChatMessage message);

    /// <summary>
    /// 获取会话的所有消息
    /// </summary>
    /// <param name="sessionId">会话ID</param>
    /// <returns>消息列表</returns>
    List<ChatMessage> GetMessages(string sessionId);

    /// <summary>
    /// 清空会话
    /// </summary>
    /// <param name="sessionId">会话ID</param>
    void ClearSession(string sessionId);

    /// <summary>
    /// 更新消息
    /// </summary>
    /// <param name="sessionId">会话ID</param>
    /// <param name="messageId">消息ID</param>
    /// <param name="updateAction">更新操作</param>
    void UpdateMessage(string sessionId, string messageId, Action<ChatMessage> updateAction);

    /// <summary>
    /// 获取消息
    /// </summary>
    /// <param name="sessionId">会话ID</param>
    /// <param name="messageId">消息ID</param>
    /// <returns>消息</returns>
    ChatMessage? GetMessage(string sessionId, string messageId);
}

