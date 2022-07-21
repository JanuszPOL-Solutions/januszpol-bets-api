using JanuszPOL.JanuszPOLBets.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JanuszPOL.JanuszPOLBets.Repository.Games.Dto
{
    public class GetGameResultDto
    {
        public long GameId { get; set; }
        public string Team1Name { get; set; }
        public string Team2Name { get; set; }
        private string _winner { get; set; }
        public string Winner 
        { 
            get
            {
                return _winner;
            }

            set
            {
                if (value == null)
                    _winner = "TBD";
                else
                    _winner = value;
            }
        }
    }
}
