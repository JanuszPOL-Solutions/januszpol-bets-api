using JanuszPOL.JanuszPOLBets.Data.Entities;

namespace JanuszPOL.JanuszPOLBets.Repository.Games.Dto;

public class SimpleGameDto
{
    public long Id { get; set; }
    public DateTime GameDate { get; set; }
    public string PhaseName { get; set; }
    public Phase.Types PhaseId { get; set; }
    public string Team1Name { get; set; }
    public string Team2Name { get; set; }
    public int? Team1Score { get; set; }
    public int? Team2Score { get; set; }
    public List<GameEvent> Events { get; set; }

    public class GameEvent
    {
        public long EventId { get; set; }
        public string Name { get; set; }
        public bool? Happened { get; set; }
    }
}
