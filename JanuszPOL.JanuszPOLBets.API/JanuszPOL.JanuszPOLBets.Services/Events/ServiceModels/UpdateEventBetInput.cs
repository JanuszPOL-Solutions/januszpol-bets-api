using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JanuszPOL.JanuszPOLBets.Services.Events.ServiceModels
{
    public class UpdateEventBetInput
    {
        [Required]
        public long GameId { get; set; }
        [Required]
        public long EventId { get; set; }
        public int? Score1 { get; set; }
        public int? Score2 { get; set; }
    }
}
