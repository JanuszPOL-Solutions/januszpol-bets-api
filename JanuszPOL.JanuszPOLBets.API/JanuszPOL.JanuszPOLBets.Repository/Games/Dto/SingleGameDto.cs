using JanuszPOL.JanuszPOLBets.Data.Entities;
using JanuszPOL.JanuszPOLBets.Data.Entities.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JanuszPOL.JanuszPOLBets.Repository.Games.Dto
{
    public class SingleGameDto
    {
        public long Id { get; set; }
        public string Team1Name { get; set; }
        public string Team2Name { get; set; }
        public DateTime GameDate { get; set; }
        public int? Team1Score { get; set; }
        public int? Team2Score { get; set; }
        public int? Team1ScoreExtraTime { get; set; }
        public int? Team2ScoreExtraTime { get; set; }
        public int? Team1ScorePenalties { get; set; }
        public int? Team2ScorePenalties { get; set; }
        public string PhaseName { get; set; }
        public Phase.Types PhaseId { get; set; }
        public GameResult.Values GameResultId { get; set; }
        public List<EventDto> SelectedEvents { get; set; }


    }

    public class EventDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int BetCost { get; set; }
        public int? GainedPoints { get; set; }
        public long EventTypeId { get; set; }
        public int? Team1Score { get; set; }
        public int? Team2Score { get; set; }
        public long? MatchResult { get; set; }
    }
}
