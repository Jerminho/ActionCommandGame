using ActionCommandGame.DTO.Results;
using ActionCommandGame.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ActionCommandGame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NegativeGameEventController : ControllerBase
    {
        private readonly INegativeGameEventService _negativeGameEventService;

        public NegativeGameEventController(INegativeGameEventService negativeGameEventService)
        {
            _negativeGameEventService = negativeGameEventService;
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandomNegativeGameEvent()
        {
            var result = await _negativeGameEventService.GetRandomNegativeGameEvent();
            if (!result.IsSuccess || result.Data == null)
            {
                return NotFound(result.Messages);
            }

            return Ok(result.Data);
        }
    }
}
