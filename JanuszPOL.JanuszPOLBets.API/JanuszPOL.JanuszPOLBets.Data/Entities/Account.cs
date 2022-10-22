using Microsoft.AspNetCore.Identity;

namespace JanuszPOL.JanuszPOLBets.Data.Entities;

public class Account : IdentityUser<long>
{
    public DateTime LastLoginDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}