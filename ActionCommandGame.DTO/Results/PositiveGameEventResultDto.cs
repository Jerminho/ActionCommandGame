using ActionCommandGame.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionCommandGame.DTO.Results
{
    public class PositiveGameEventResultDto : IHasProbability
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int Money { get; set; }
        public int Experience { get; set; }
        public int Probability { get; set; }
    }
}
