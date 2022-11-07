namespace JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels
{
    // For now we support only bool events
    public class EventBetResultInput
    {
        public long EventId { get; set; }
        public long GameId { get; set; }
    }
}
