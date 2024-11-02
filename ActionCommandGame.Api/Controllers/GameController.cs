using ActionCommandGame.DTO.Results;
using ActionCommandGame.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ActionCommandGame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost("perform-action/{playerId}")]
        public async Task<IActionResult> PerformAction(int playerId)
        {
            var result = await _gameService.PerformAction(playerId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Messages);
            }
            return Ok(result.Data);
        }

        [HttpPost("buy/{playerId}/{itemId}")]
        public async Task<IActionResult> Buy(int playerId, int itemId)
        {
            var result = await _gameService.Buy(playerId, itemId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Messages);
            }
            return Ok(result.Data);
        }
    }
}
