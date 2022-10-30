using Entities = JanuszPOL.JanuszPOLBets.Data.Entities.Events;

namespace JanuszPOL.JanuszPOLBets.Repository.Events.Dto
{
    public class EventDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BetCost { get; set; }
        public int WinValue { get; set; }
        public Entities.EventType.RuleType EventType { get; set; }
    }
}
