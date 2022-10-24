namespace JanuszPOL.JanuszPOLBets.Data.Entities.Events
{
    public class EventType
    {
        public enum RuleType
        {
            BaseBet = 1,
            Boolean = 2,
            ExactValue = 3,
            TwoExactValues = 4
        }

        public long Id { get; set; }
        public RuleType Type { get; set; }
    }
}


