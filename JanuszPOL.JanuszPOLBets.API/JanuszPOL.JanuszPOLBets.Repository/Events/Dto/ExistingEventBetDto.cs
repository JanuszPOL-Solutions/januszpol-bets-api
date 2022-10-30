using JanuszPOL.JanuszPOLBets.Data.Entities.Events;

namespace JanuszPOL.JanuszPOLBets.Repository.Events.Dto
{
    public class ExistingEventBetDto : AddEventBetDto
    {
        public long EventBetId { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public EventType.RuleType EventType { get; set; }    
    }
}
