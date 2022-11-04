using System.Text.Json.Serialization;

namespace JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels
{
    public class BaseEventBetInput
    {
        public long GameId { get; set; }
        public BaseBetType BetType { get; set; }

        [JsonIgnore]
        public long AccountId { get; set; }
    }
}
