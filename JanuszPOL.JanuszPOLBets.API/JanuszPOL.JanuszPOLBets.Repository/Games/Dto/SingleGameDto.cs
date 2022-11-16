using JanuszPOL.JanuszPOLBets.Data.Entities;

namespace JanuszPOL.JanuszPOLBets.Repository.Games.Dto
{
    public class SingleGameDto
    {
        public long Id { get; set; }
        public string Team1 { get; set; }
        public string Team2 { get; set; }
        public DateTime GameDate { get; set; }
        public int? Team1Score { get; set; }
        public int? Team2Score { get; set; }
        public int? Team1ScoreExtraTime { get; set; }
        public int? Team2ScoreExtraTime { get; set; }
        public int? Team1ScorePenalties { get; set; }
        public int? Team2ScorePenalties { get; set; }
        public string PhaseName { get; set; }
        public Phase.Types PhaseId { get; set; }
        public GameResult.Values? GameResultId { get; set; }
        public bool Started { get; set; }

    }
}
