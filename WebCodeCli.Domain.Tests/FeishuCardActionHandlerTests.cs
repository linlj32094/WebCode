using System.Reflection;
using FeishuNetSdk.CallbackEvents;
using Xunit;

namespace WebCodeCli.Domain.Tests;

public class FeishuCardActionHandlerTests
{
    [Theory]
    [InlineData("{\"action\":\"switch_session\",\"session_id\":\"session-2\"}", "{\"action\":\"switch_session\",\"session_id\":\"session-2\"}")]
    [InlineData("\"{\\\"action\\\":\\\"switch_session\\\",\\\"session_id\\\":\\\"session-2\\\"}\"", "{\"action\":\"switch_session\",\"session_id\":\"session-2\"}")]
    [InlineData("  \"{\\\"action\\\":\\\"open_session_manager\\\"}\"  ", "{\"action\":\"open_session_manager\"}")]
    public void NormalizeActionPayload_UnwrapsOverflowPayloadShapes(string rawPayload, string expected)
    {
        var method = typeof(WebCodeCli.Domain.Domain.Service.Channels.FeishuCardActionHandler)
            .GetMethod("NormalizeActionPayload", BindingFlags.NonPublic | BindingFlags.Static);

        Assert.NotNull(method);

        var normalized = (string?)method!.Invoke(null, [rawPayload]);

        Assert.Equal(expected, normalized);
    }

    [Theory]
    [InlineData("overflow", "{\"action\":\"switch_session\",\"session_id\":\"session-2\"}", CardActionTriggerResponseDto.ToastSuffix.ToastType.Success, true)]
    [InlineData("button", "{\"action\":\"switch_session\",\"session_id\":\"session-2\"}", CardActionTriggerResponseDto.ToastSuffix.ToastType.Success, false)]
    [InlineData("overflow", "{\"action\":\"switch_session\",\"session_id\":\"session-2\"}", CardActionTriggerResponseDto.ToastSuffix.ToastType.Error, false)]
    [InlineData("overflow", "{\"action\":\"open_session_manager\"}", CardActionTriggerResponseDto.ToastSuffix.ToastType.Success, false)]
    public void ShouldSuppressOverflowSwitchSessionSuccessResponse_MatchesExpectedCases(
        string actionTag,
        string actionValue,
        CardActionTriggerResponseDto.ToastSuffix.ToastType toastType,
        bool expected)
    {
        var method = typeof(WebCodeCli.Domain.Domain.Service.Channels.FeishuCardActionHandler)
            .GetMethod("ShouldSuppressOverflowSwitchSessionSuccessResponse", BindingFlags.NonPublic | BindingFlags.Static);

        Assert.NotNull(method);

        var response = new CardActionTriggerResponseDto
        {
            Toast = new CardActionTriggerResponseDto.ToastSuffix
            {
                Type = toastType,
                Content = "toast"
            }
        };

        var actual = (bool?)method!.Invoke(null, [actionTag, actionValue, response]);

        Assert.Equal(expected, actual);
    }
}
