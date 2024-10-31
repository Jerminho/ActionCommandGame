using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionCommandGame.DTO.Results
{
    public class BuyResult
    {
        public PlayerResult? Player { get; set; }
        public ItemResult? Item { get; set; }
    }
}
