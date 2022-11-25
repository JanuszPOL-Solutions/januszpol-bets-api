using JanuszPOL.JanuszPOLBets.Data.Entities;

namespace JanuszPOL.JanuszPOLBets.Repository.Games.Dto;

public class GameBetsDto
{
    public long GameId { get; set; }
    public DateTime GameDate { get; set; }
    public string Team1Name { get; set; }
    public string Team2Name { get; set; }
    public int? Team1Score { get; set; }
    public int? Team2Score { get; set; }
    public Phase.Types PhaseId { get; set; }
    public string PhaseName { get; set; }
    public List<Bet> Bets { get; set; }

    public class Bet
    {
        public string Username { get; set; }
        public long? ExactScoreTeam1 { get; set; }
        public long? ExactScoreTeam2 { get; set; }
        public bool? ExactScoreResult { get; set; }
        public long? ResultBet { get; set; }
        public bool? ResultBetResult { get; set; }
        public BoolBet[] BoolBets { get; set; }

        public class BoolBet
        {
            public string Name { get; set; }
            public bool? Result { get; set; }
            public int Cost { get; set; }
            public int WinValue { get; set; }
        }
    }
}
