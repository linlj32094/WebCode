using WebCodeCli.Domain.Domain.Model;
using WebCodeCli.Domain.Domain.Model.Channels;

namespace WebCodeCli.Domain.Domain.Service.Channels;

internal static class SuperpowersQuickActionCardHelper
{
    public static bool ShouldShowPlanActions(
        IEnumerable<string?>? sessionContents,
        bool hasPlanFiles)
    {
        return hasPlanFiles
               && SessionContainsSuperpowers(sessionContents);
    }

    public static FeishuStreamingCardBottomPrompt CreateBottomPrompt(
        string sessionId,
        string chatKey,
        string? toolId)
    {
        return new FeishuStreamingCardBottomPrompt
        {
            InputName = SuperpowersQuickActionDefaults.QuickInputFieldName,
            InputLabel = SuperpowersQuickActionDefaults.InstructionText,
            Placeholder = SuperpowersQuickActionDefaults.QuickInputPlaceholder,
            DefaultValue = string.Empty,
            ButtonText = SuperpowersQuickActionDefaults.QuickSubmitButtonText,
            ButtonType = "primary",
            Value = BuildActionValue(
                FeishuHelpCardAction.SubmitSuperpowersQuickInputAction,
                sessionId,
                chatKey,
                toolId)
        };
    }

    public static IReadOnlyList<FeishuStreamingCardBottomAction> CreateBottomActions(
        string sessionId,
        string chatKey,
        string? toolId)
    {
        return
        [
            new FeishuStreamingCardBottomAction
            {
                Text = SuperpowersQuickActionDefaults.ExecutePlanButtonText,
                Type = "default",
                Value = BuildActionValue(
                    FeishuHelpCardAction.ExecuteSuperpowersPlanAction,
                    sessionId,
                    chatKey,
                    toolId)
            },
            new FeishuStreamingCardBottomAction
            {
                Text = SuperpowersQuickActionDefaults.ExecuteSubagentPlanButtonText,
                Type = "primary",
                Value = BuildActionValue(
                    FeishuHelpCardAction.ExecuteSuperpowersSubagentPlanAction,
                    sessionId,
                    chatKey,
                    toolId)
            }
        ];
    }

    private static object BuildActionValue(
        string action,
        string sessionId,
        string chatKey,
        string? toolId)
    {
        return new
        {
            action,
            session_id = sessionId,
            chat_key = chatKey,
            tool_id = toolId
        };
    }

    private static bool SessionContainsSuperpowers(IEnumerable<string?>? sessionContents)
    {
        return sessionContents != null
               && sessionContents.Any(content =>
                   !string.IsNullOrWhiteSpace(content)
                   && content.Contains("superpowers", StringComparison.OrdinalIgnoreCase));
    }
}
