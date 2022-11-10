using Microsoft.AspNetCore.Identity;

namespace JanuszPOL.JanuszPOLBets.Services.Account.Extensions;

public static class IdentityErrorExtensions
{
    public static string GetErrorMessage(this IdentityError identityError)
    {
        switch (identityError.Code)
        {
            case "PasswordRequiresDigit":
                return "Hasło musi mieć liczbę";
            case "PasswordTooShort":
                return "Hasło musi mieć co najmniej 6 znaków";
            default:
                return "Nie udało się stworzyć użytkownika. Spróbuj inny login lub hasło";
        }
    }
}
