using System.Text.Json.Serialization;

namespace JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels;

public class BoolBetInput
{
    [JsonIgnore]
    public long AccountId { get; set; }
    public long GameId { get; set; }
    public long EventId { get; set; }
}
