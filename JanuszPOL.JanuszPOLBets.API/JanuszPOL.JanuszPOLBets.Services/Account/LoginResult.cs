namespace JanuszPOL.JanuszPOLBets.Services.Account;

public class AccountResult
{
    public string Username { get; set; }
    public bool IsAdmin { get; set; }
}

public class LoginResult : AccountResult
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}
