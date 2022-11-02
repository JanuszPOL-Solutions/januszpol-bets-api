namespace JanuszPOL.JanuszPOLBets.Services.Account;

public class LoginResult
{
    public LoginResult(string token, string username, bool isAdmin)
    {
        Token = token;
        Username = username;
        IsAdmin = isAdmin;
    }

    public string Token { get; }
    public string Username { get; }
    public bool IsAdmin { get; }
}
