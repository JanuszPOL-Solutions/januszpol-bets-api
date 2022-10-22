using JanuszPOL.JanuszPOLBets.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JanuszPOL.JanuszPOLBets.Repository.Games.Dto
{
    public class UpdateGameDto
    {
        public long Id { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public int? Team1ScoreExtraTime { get; set; }
        public int? Team2ScoreExtraTime { get; set; }
        public int? Team1ScorePenalties { get; set; }
        public int? Team2ScorePenalties { get; set; }
        public GameResult.Values GameResultId { get; set; }
    }
}
