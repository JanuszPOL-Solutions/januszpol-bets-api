using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JanuszPOL.JanuszPOLBets.Data.Entities
{
    public class Game
    {
        public long GameId { get; set; }
        public Team Team1Id { get; set; }
        public Team Team2Id { get; set; }
        public DateTime GameDate { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public int? Team1ScoreExtraTime { get; set; }
        public int? Team2ScoreExtraTime { get; set; }
        public int? Team1ScorePenalties { get; set; }
        public int? Team2ScorePenalties { get; set; }
        public GameResult GameResult { get; set; }
    }

    public class GameResult
    {
        public int GameResultId { get; set; }
        public string Name { get; set; }
    }
}
