using JanuszPOL.JanuszPOLBets.Data.Entities;

namespace JanuszPOL.JanuszPOLBets.Services.Interfaces;

public interface ITokenService
{
    string CreateToken(Account account);
}