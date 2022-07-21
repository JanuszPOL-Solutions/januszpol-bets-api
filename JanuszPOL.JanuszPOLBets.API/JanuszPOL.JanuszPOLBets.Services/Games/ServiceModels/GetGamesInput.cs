using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JanuszPOL.JanuszPOLBets.Services.Games.ServiceModels
{
    public class GetGamesInput
    {
        [MaxLength(64)]
        public string NameContains { get; set; }

        [MaxLength(64)]
        public string NameStartsWith { get; set; }

        [Required]
        public int Skip { get; set; }

        [Required]
        public int Limit { get; set; }
    }
}
