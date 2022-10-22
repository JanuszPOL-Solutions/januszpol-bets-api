
namespace JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels
{
    public class EventBase
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BetCost { get; set; }
        public int WinValue { get; set; }
        public EventType EventType { get; set; }

        public static EventType TranslateEventType(JanuszPOL.JanuszPOLBets.Repository.Events.Dto.EventType eventType)
        {
            switch(eventType)
            {
                case Repository.Events.Dto.EventType.BaseBet:
                    return EventType.BaseBet;

                case Repository.Events.Dto.EventType.Boolean:
                    return EventType.Boolean;

                case Repository.Events.Dto.EventType.ExactValue:
                    return EventType.ExactValue;

                case Repository.Events.Dto.EventType.TwoExactValues:
                    return EventType.TwoExactValues;

                default:
                    throw new Exception($"Invalid event type, {eventType}");
            }
        }
    }
}
