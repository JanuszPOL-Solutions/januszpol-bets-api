using System.ComponentModel.DataAnnotations;

namespace JanuszPOL.JanuszPOLBets.Services.Teams.ServiceModels;

public class GetTeamsInput
{
    [MaxLength(64)]
    public string NameContains { get; set; }

    [MaxLength(64)]
    public string NameStartsWith { get; set; }

    public int Skip { get; set; }

    public int Limit { get; set; }
}
