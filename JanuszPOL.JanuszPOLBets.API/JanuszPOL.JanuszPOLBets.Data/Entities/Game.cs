namespace JanuszPOL.JanuszPOLBets.Data.Entities;

public class Game
{
    public long Id { get; set; }
    public long Team1Id { get; set; }
    public long Team2Id { get; set; }
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

    public Team Team1 { get; set; }
    public Team Team2 { get; set; }
}
