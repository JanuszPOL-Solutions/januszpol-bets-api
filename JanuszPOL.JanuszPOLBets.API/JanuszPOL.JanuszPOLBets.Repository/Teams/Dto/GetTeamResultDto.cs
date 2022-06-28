using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JanuszPOL.JanuszPOLBets.Repository.Teams.Dto
{
    public class GetTeamResultDto
    {
        public long TeamId { get; set; }
        public string Name { get; set; }
        public string FlagUrl { get; set; }
    }
}
