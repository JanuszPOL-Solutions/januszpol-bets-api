using JanuszPOL.JanuszPOLBets.Data.Entities.Events;

namespace JanuszPOL.JanuszPOLBets.Data.Extensions;

public static class EventExtensions
{
    public static bool IsBaseEvent(this Event @event)
    {
        return @event.EventTypeId == EventType.RuleType.BaseBet;
    }
}
