using ActionCommandGame.DTO.Results;
using ActionCommandGame.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ActionCommandGame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerItemController : ControllerBase
    {
        private readonly IPlayerItemService _playerItemService;

        public PlayerItemController(IPlayerItemService playerItemService)
        {
            _playerItemService = playerItemService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayerItem(int id)
        {
            var result = await _playerItemService.Get(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.Messages);
            }
            return Ok(result.Data);
        }

        [HttpPost("create/{playerId}/{itemId}")]
        public async Task<IActionResult> CreatePlayerItem(int playerId, int itemId)
        {
            var result = await _playerItemService.Create(playerId, itemId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Messages);
            }
            return CreatedAtAction(nameof(GetPlayerItem), new { id = result.Data.Id }, result.Data);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePlayerItem(int id)
        {
            var result = await _playerItemService.Delete(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.Messages);
            }
            return NoContent();
        }
    }
}
