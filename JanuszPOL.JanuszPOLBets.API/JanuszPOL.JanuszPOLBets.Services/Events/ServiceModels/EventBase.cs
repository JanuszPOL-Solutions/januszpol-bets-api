namespace JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels
{
    public class EventBase
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BetCost { get; set; }
        public int WinValue { get; set; }
    }
}
