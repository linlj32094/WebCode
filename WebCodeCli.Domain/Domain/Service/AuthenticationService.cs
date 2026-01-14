using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebCodeCli.Domain.Common.Extensions;
using WebCodeCli.Domain.Common.Options;

namespace WebCodeCli.Domain.Domain.Service;

/// <summary>
/// 认证服务实现
/// </summary>
[ServiceDescription(typeof(IAuthenticationService), ServiceLifetime.Scoped)]
public class AuthenticationService : IAuthenticationService
{
    private readonly AuthenticationOption _authOption;

    public AuthenticationService(IOptions<AuthenticationOption> authOption)
    {
        _authOption = authOption.Value;
    }

    public bool ValidateUser(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        var user = _authOption.Users.FirstOrDefault(u => 
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        if (user == null)
        {
            return false;
        }

        return user.Password == password;
    }

    public bool IsAuthenticationEnabled()
    {
        return _authOption.Enabled;
    }
}

