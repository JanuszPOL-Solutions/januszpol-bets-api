namespace JanuszPOL.JanuszPOLBets.Repository.Events.Dto
{
    public class EventBetResultDto : EventBetDto
    {
        public bool? Result { get; set; }
        public int BetCost { get; set; }
        public int WinValue { get; set; }
    }
}
