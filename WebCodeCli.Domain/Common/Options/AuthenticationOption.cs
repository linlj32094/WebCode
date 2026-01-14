namespace WebCodeCli.Domain.Common.Options;

public class AuthenticationOption
{
    public bool Enabled { get; set; } = true;
    public List<UserCredential> Users { get; set; } = new();
}

public class UserCredential
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

