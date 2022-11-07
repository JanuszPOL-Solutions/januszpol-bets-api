namespace JanuszPOL.JanuszPOLBets.Data.Entities.Events
{
    public class Event
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BetCost { get; set; }
        public int WinValue { get; set; }
        public EventType.RuleType EventTypeId { get; set; }
        public virtual EventType EventType { get; set; }
    }

}
