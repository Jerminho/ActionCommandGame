using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionCommandGame.DTO.Results
{
    public class BuyResultDto
    {
        public PlayerResultDto? Player { get; set; }
        public ItemResultDto? Item { get; set; }
    }
}
