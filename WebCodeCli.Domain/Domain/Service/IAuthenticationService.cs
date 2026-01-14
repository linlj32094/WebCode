namespace WebCodeCli.Domain.Domain.Service;

public interface IAuthenticationService
{
    /// <summary>
    /// 验证用户登录
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <returns>登录是否成功</returns>
    bool ValidateUser(string username, string password);
    
    /// <summary>
    /// 检查认证是否启用
    /// </summary>
    /// <returns>true表示启用认证</returns>
    bool IsAuthenticationEnabled();
}

