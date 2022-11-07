using JanuszPOL.JanuszPOLBets.Repository.Games.Dto;

namespace JanuszPOL.JanuszPOLBets.Services.Games.ServiceModels;

public class GetGamesResult
{
    public long Id { get; set; }
    public string Team1 { get; set; }
    public string Team2 { get; set; }
    public DateTime GameDate { get; set; }
    public int? Team1Score { get; set; }
    public int? Team1PenaltyScore { get; set; }
    public int? Team2Score { get; set; }
    public int? Team2PenaltyScore { get; set; }
    public string PhaseName { get; set; }
    public int Phase { get; set; }
    public int? Result { get; set; }

    public GameEventBetDto ResultEvent { get; set; }
    public int EventsBetedCount { get; set; }
}
