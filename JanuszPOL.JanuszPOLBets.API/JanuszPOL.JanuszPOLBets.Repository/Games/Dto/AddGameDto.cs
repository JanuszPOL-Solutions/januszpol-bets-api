using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JanuszPOL.JanuszPOLBets.Repository.Games.Dto
{
    public class AddGameDto
    {
        public long Team1Id { get; set; }
        public long Team2Id { get; set; }
        public DateTime GameDate { get; set; }
    }
}
