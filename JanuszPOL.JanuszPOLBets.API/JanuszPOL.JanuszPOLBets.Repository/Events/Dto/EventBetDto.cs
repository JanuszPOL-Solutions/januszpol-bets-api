namespace JanuszPOL.JanuszPOLBets.Repository.Events.Dto
{
    public class EventBetDto
    {
        public long EventId { get; set; }
        public long GameId { get; set; }
        public long AccountId { get; set; }
        public int? Value1 { get; set; }
        public int? Value2 { get; set; }
        public bool? Result { get; set; }
    }
}
