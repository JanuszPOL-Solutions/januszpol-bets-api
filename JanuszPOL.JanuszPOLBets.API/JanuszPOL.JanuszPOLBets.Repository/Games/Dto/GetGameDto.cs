using JanuszPOL.JanuszPOLBets.Data.Entities;

namespace JanuszPOL.JanuszPOLBets.Repository.Games.Dto;

public class GetGameDto
{
    public BetState? Beted { get; set; }
    public Phase.Types? Phase { get; set; }
    public string[]? PhaseNames { get; set; }
    public long[]? TeamIds { get; set; }
    public long AccountId { get; set; }

    public enum BetState
    {
        NotBeted = 0,
        Beted = 1,
        ToBet = 2
    }
}
