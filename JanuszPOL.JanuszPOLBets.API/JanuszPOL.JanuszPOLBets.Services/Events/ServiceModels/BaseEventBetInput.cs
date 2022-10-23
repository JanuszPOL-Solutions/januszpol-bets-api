namespace JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels
{
    public class BaseEventBetInput
    {
        public long GameId { get; set; }
        public long AccountId { get; set; }
        public BaseBetType BetType { get; set; }
    }
}
