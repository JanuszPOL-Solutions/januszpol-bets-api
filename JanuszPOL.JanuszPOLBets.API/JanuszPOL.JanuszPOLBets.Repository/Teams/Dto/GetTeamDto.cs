namespace JanuszPOL.JanuszPOLBets.Repository.Teams.Dto
{
    public class GetTeamDto
    {
        public string NameContains { get; set; }
        public string NameStartsWith { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}
