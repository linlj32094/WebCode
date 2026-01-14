using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using WebCodeCli.Domain.Domain.Service;

namespace WebCodeCli.Pages;

public partial class Login : ComponentBase
{
    [Inject] private IAuthenticationService AuthenticationService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _isLoading = false;
    private bool _showPassword = false;

    protected override async Task OnInitializedAsync()
    {
        // 如果认证未启用，直接跳转到主页
        if (!AuthenticationService.IsAuthenticationEnabled())
        {
            NavigationManager.NavigateTo("/code-assistant");
            return;
        }

        // 检查是否已登录
        var isAuthenticated = await CheckAuthenticationAsync();
        if (isAuthenticated)
        {
            NavigationManager.NavigateTo("/code-assistant");
        }
    }

    private async Task HandleLogin()
    {
        _errorMessage = string.Empty;

        // 验证输入
        if (string.IsNullOrWhiteSpace(_username))
        {
            _errorMessage = "请输入用户名";
            return;
        }

        if (string.IsNullOrWhiteSpace(_password))
        {
            _errorMessage = "请输入密码";
            return;
        }

        _isLoading = true;
        StateHasChanged();

        // 模拟网络延迟，提升用户体验
        await Task.Delay(300);

        try
        {
            // 验证用户名和密码
            bool isValid = AuthenticationService.ValidateUser(_username, _password);

            if (isValid)
            {
                // 登录成功，保存认证状态到 SessionStorage
                await SaveAuthentication(_username);
                
                // 跳转到主页
                NavigationManager.NavigateTo("/code-assistant", forceLoad: true);
            }
            else
            {
                _errorMessage = "用户名或密码错误";
                _password = string.Empty; // 清空密码
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"登录失败: {ex.Message}";
            Console.WriteLine($"登录异常: {ex}");
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter" && !_isLoading)
        {
            await HandleLogin();
        }
    }

    private void TogglePasswordVisibility()
    {
        _showPassword = !_showPassword;
    }

    private async Task SaveAuthentication(string username)
    {
        try
        {
            await JSRuntime.InvokeVoidAsync("sessionStorage.setItem", "isAuthenticated", "true");
            await JSRuntime.InvokeVoidAsync("sessionStorage.setItem", "username", username);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"保存认证状态失败: {ex.Message}");
        }
    }

    private async Task<bool> CheckAuthenticationAsync()
    {
        try
        {
            var isAuthenticated = await JSRuntime.InvokeAsync<string>("sessionStorage.getItem", "isAuthenticated");
            return isAuthenticated == "true";
        }
        catch
        {
            return false;
        }
    }
}

