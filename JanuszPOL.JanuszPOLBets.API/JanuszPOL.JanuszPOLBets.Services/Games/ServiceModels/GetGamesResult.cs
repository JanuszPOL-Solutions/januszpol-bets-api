namespace JanuszPOL.JanuszPOLBets.Services.Games.ServiceModels;

public class GetGamesResult
{
    public long GameId { get; set; }
    public string Team1Name { get; set; }
    public string Team2Name { get; set; }
    public string? Winner { get; set; }
}
