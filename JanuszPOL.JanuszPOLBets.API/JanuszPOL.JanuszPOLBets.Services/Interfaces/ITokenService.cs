using JanuszPOL.JanuszPOLBets.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JanuszPOL.JanuszPOLBets.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(Account account);
    }
}