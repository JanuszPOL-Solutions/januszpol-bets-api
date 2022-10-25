using JanuszPOL.JanuszPOLBets.Data.Entities;
using System.Text.Json.Serialization;
using static JanuszPOL.JanuszPOLBets.Repository.Games.Dto.GetGameDto;

namespace JanuszPOL.JanuszPOLBets.Services.Games.ServiceModels;

public class GetGamesInput
{
    [JsonIgnore]
    public long AccountId { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BetState? Beted { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Phase.Types? Phase { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? PhaseNames { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long[]? TeamIds { get; set; }
}
