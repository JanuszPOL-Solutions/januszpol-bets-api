namespace JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels
{
    public class EventBet
    {
        public long BetId { get; set; }
        public long GameId { get; set; }
        public long AccountId { get; set; }
        public EventType EventType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Value1 { get; set; }
        public int? Value2 { get; set; }
    }
}
