using ActionCommandGame.DTO.Results;
using ActionCommandGame.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ActionCommandGame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PositiveGameEventController : ControllerBase
    {
        private readonly IPositiveGameEventService _positiveGameEventService;

        public PositiveGameEventController(IPositiveGameEventService positiveGameEventService)
        {
            _positiveGameEventService = positiveGameEventService;
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandomPositiveGameEvent([FromQuery] bool hasAttackItem)
        {
            var result = await _positiveGameEventService.GetRandomPositiveGameEvent(hasAttackItem);
            if (!result.IsSuccess || result.Data == null)
            {
                return NotFound(result.Messages);
            }

            return Ok(result.Data);
        }
    }
}
