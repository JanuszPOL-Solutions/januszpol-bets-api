using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JanuszPOL.JanuszPOLBets.Data.Entities
{
    public class Phase
    {
        public Types Id { get; set; }
        public string Name { get; set; }
        public enum Types
        {
            Group = 0,
            Playoffs = 1
        }
    }
}
