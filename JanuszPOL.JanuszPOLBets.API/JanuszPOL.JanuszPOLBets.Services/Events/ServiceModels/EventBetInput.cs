using System.ComponentModel.DataAnnotations;

namespace JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels
{
    public class EventBetInput
    {
        public long EventId { get; set; }
        public long GameId { get; set; }
        public long AccountId { get; set; }
        public int? Value1 { get; set; }
        public int? Value2 { get; set; }
    }
}
