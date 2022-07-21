using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JanuszPOL.JanuszPOLBets.Repository.Games.Dto
{
    public class GetGameDto
    {
        public string NameContains { get; set; }
        public string NameStartsWith { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}
