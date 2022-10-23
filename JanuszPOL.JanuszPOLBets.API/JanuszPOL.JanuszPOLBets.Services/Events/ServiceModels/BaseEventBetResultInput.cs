namespace JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels
{
    public class BaseEventBetResultInput
    {
        public long GameId { get; set; }
        public BaseBetType BetType { get; set; }
    }
}
