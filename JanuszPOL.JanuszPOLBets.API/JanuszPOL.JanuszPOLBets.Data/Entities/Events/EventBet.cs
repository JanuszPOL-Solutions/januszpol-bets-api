namespace JanuszPOL.JanuszPOLBets.Data.Entities.Events
{
    public class EventBet
    {
        public long Id { get; set; }
        public long GameId { get; set; }
        public long EventId { get; set; }
        public long AccountId { get; set; }
        public int? Value1 { get; set; }
        public int? Value2 { get; set; }
        public bool? Result { get; set; }
        public bool IsDeleted { get; set; }

        public Event Event { get; set; }
        public Game Game { get; set; }
        public Account Account { get; set; }
    }
}
