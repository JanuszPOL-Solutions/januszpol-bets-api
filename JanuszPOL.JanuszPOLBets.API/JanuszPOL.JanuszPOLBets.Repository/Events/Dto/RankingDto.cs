namespace JanuszPOL.JanuszPOLBets.Repository.Events.Dto;

public class RankingDto
{
    public RankingRow[] Rows { get; set; }

    public class RankingRow
    {
        public string Username { get; set; }
        public int Score { get; set; }
    }
}
