namespace JanuszPOL.JanuszPOLBets.Data.Entities.Events
{
    public class EventType
    {
        public enum RuleType
        {
            BaseBet,
            Boolean,
            ExactValue,
            TwoExactValues
        }

        public long Id { get; set; }
        public RuleType Type { get; set; }
    }
}
