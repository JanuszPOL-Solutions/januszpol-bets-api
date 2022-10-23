using JanuszPOL.JanuszPOLBets.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JanuszPOL.JanuszPOLBets.Repository.Games.Dto
{
    public class UpdateGameInput
    {
        [Required]
        public long Id { get; set; }
        [Required]
        [Range(0, 20)]
        public int Team1Score { get; set; }
        [Required]
        [Range(0, 20)]
        public int Team2Score { get; set; }
        [Range(0, 20)]
        public int? Team1ScoreExtraTime { get; set; }
        [Range(0, 20)]
        public int? Team2ScoreExtraTime { get; set; }
        [Range(0, 20)]
        public int? Team1ScorePenalties { get; set; }
        [Range(0, 20)]
        public int? Team2ScorePenalties { get; set; }
        [Required]
        [Range(0, 6)]
        public GameResult.Values GameResultId { get; set; }
    }
}
