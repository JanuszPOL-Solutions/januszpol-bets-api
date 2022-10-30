using System.Text.Json.Serialization;

namespace JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels
{
    public class TwoValuesEventBetInput
    {
        public long AccountId { get; set; }
        public long? EventBetId { get; set; }
        public long GameId { get; set; }
        public int Value1 { get; set; }
        public int Value2 { get; set; }

        [JsonIgnore]
        public long EventId { get; set; }
    }
}
