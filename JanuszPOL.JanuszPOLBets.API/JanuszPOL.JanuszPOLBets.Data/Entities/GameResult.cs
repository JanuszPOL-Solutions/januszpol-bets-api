namespace JanuszPOL.JanuszPOLBets.Data.Entities;

public class GameResult
{
    public Values Id { get; set; }
    public string Name { get; set; }
    public enum Values
    {
        Draw = 0,
        Team1 = 1,
        Team2 = 2,
        Team1ExtraTime = 3,
        Team2ExtraTime = 4,
        Team1Penalties = 5,
        Team2Penalties = 6
    }
}
