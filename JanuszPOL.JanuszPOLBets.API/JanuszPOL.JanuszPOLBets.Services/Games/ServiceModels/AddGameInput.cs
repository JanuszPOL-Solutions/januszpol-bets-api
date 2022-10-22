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
    }
}
