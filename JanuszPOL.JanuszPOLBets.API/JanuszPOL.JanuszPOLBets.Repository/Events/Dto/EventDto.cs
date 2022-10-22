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
            switch(eventType.Type)
            {
                case Entities.EventType.RuleType.BaseBet:
                    return EventType.BaseBet;
                case Entities.EventType.RuleType.Boolean:
                    return EventType.Boolean;
                case Entities.EventType.RuleType.ExactValue:
                    return EventType.ExactValue;
                case Entities.EventType.RuleType.TwoExactValues:
                    return EventType.TwoExactValues;

                default:
                    throw new Exception($"Invalid type {eventType.Type}");
            }
        }
    }
}
