using ActionCommandGame.DTO.Filters;
using ActionCommandGame.DTO.Results;
using ActionCommandGame.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActionCommandGame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayer(int id)
        {
            var result = await _playerService.Get(id);
            if (!result.IsSuccess || result.Data == null)
            {
                return NotFound(result.Messages);
            }

            return Ok(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayers([FromQuery] PlayerFilterDto filter)
        {
            var result = await _playerService.Find(filter);
            return Ok(result.Data);
        }
    }
}
