using ActionCommandGame.DTO.Results;
using ActionCommandGame.Model;
using ActionCommandGame.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActionCommandGame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var result = await _itemService.Get(id);
            if (!result.IsSuccess || result.Data == null)
            {
                return NotFound(result.Messages);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] Item item)
        {
            var result = await _itemService.Create(item);
            return CreatedAtAction(nameof(GetItem), new { id = result.Data.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] Item item)
        {
            item.Id = id; // Ensure item ID matches URL
            var result = await _itemService.Update(item);
            if (!result.IsSuccess)
            {
                return NotFound(result.Messages);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var result = await _itemService.Delete(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.Messages);
            }

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            var result = await _itemService.Find();
            return Ok(result.Data);
        }
    }
}
