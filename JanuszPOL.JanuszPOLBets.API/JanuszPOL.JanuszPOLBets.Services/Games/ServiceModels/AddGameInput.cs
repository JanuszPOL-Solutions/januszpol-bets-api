using JanuszPOL.JanuszPOLBets.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JanuszPOL.JanuszPOLBets.Services.Games.ServiceModels
{
    public class AddGameInput
    {
        [Required]
        public long Team1Id { get; set; }
        [Required]
        public long Team2Id { get; set; }
        [Required]
        public DateTime GameDate { get; set; }
        [Required]
        [Range(0, 1)]
        public Phase.Types PhaseId { get; set; }
        [Required]
        public string PhaseName { get; set; }

    }
}
