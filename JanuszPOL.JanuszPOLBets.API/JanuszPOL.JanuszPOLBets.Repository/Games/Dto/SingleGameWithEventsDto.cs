using JanuszPOL.JanuszPOLBets.Data.Entities.Events;

namespace JanuszPOL.JanuszPOLBets.Repository.Games.Dto
{
    public class SingleGameWithEventsDto : SingleGameDto
    {
        public GameEventBetDto? ResultEvent { get; set; }
        public GameEventBetDto? ExactScoreEvent { get; set; }
        public List<GameEventBetDto> SelectedEvents { get; set; }
    }

    public class GameEventBetDto//TODO: cleanup a bit
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public EventType.RuleType EventTypeId { get; set; }
        public string Name { get; set; }
        public int BetCost { get; set; }
        public int? GainedPoints { get; set; }
        public int? Team1Score { get; set; }
        public int? Team2Score { get; set; }
        public long? MatchResult { get; set; }
    }
}
