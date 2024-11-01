using ActionCommandGame.Services.Model.Core; // in case I have to link back to this one
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionCommandGame.DTO.Results
{
    public class GameResultDto
    {
        public PlayerResultDto? Player { get; set; }
        public PositiveGameEventResultDto? PositiveGameEvent { get; set; }
        public NegativeGameEventResultDto? NegativeGameEvent { get; set; }
        public IList<ServiceMessage> NegativeGameEventMessages { get; set; } = new List<ServiceMessage>();
    }
}

