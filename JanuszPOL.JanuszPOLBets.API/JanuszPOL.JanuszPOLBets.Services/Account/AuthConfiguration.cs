namespace JanuszPOL.JanuszPOLBets.Services.Account;

public class AuthConfiguration
{
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public string Secret { get; set; }
    public int TokenExpirationInHours { get; set; }
}
