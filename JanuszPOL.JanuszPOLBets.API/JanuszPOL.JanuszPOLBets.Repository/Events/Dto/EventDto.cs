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
        public EventType EventType { get; set; }

        public static EventType TranslateEventType(Entities.EventType eventType)
        {
            return (EventType)(int)eventType.Type;
        }
    }
}
