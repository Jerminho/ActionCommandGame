using ActionCommandGame.DTO.Results;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Model.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActionCommandGame.Services
{
    public class ItemService : IItemService
    {
        private readonly ActionCommandGameDbContext _database;

        public ItemService(ActionCommandGameDbContext database)
        {
            _database = database;
        }

        public async Task<ServiceResult<ItemResultDto>> Get(int itemId)
        {
            var itemDto = await _database.Items
                .Where(i => i.Id == itemId)
                .ProjectToResult()
                .SingleOrDefaultAsync();

            return itemDto == null
                ? new ServiceResult<ItemResultDto>().NotFound()
                : new ServiceResult<ItemResultDto>(itemDto);
        }

        public async Task<ServiceResult<IList<ItemResultDto>>> Find()
        {
            var itemDtos = await _database.Items
                .ProjectToResult()
                .ToListAsync();

            return new ServiceResult<IList<ItemResultDto>>(itemDtos);
        }

        public async Task<ServiceResult<ItemResultDto>> Create(Item item)
        {
            _database.Items.Add(item);
            await _database.SaveChangesAsync();

            var createdItemDto = await _database.Items
                .Where(i => i.Id == item.Id)
                .ProjectToResult()
                .SingleOrDefaultAsync();

            return new ServiceResult<ItemResultDto>(createdItemDto)
            {
                Messages = new List<ServiceMessage>
                {
                    new ServiceMessage { Code = "Created", Message = "Item created successfully" }
                }
            };
        }

        public async Task<ServiceResult<ItemResultDto>> Update(Item item)
        {
            var existingItem = await _database.Items.SingleOrDefaultAsync(i => i.Id == item.Id);
            if (existingItem == null)
            {
                return new ServiceResult<ItemResultDto>().NotFound();
            }

            existingItem.Name = item.Name;
            existingItem.Description = item.Description;
            existingItem.Price = item.Price;
            existingItem.Fuel = item.Fuel;
            existingItem.Attack = item.Attack;
            existingItem.Defense = item.Defense;
            existingItem.ActionCooldownSeconds = item.ActionCooldownSeconds;

            await _database.SaveChangesAsync();

            var updatedItemDto = await _database.Items
                .Where(i => i.Id == item.Id)
                .ProjectToResult()
                .SingleOrDefaultAsync();

            return new ServiceResult<ItemResultDto>(updatedItemDto)
            {
                Messages = new List<ServiceMessage>
                {
                    new ServiceMessage { Code = "Updated", Message = "Item updated successfully" }
                }
            };
        }

        public async Task<ServiceResult<bool>> Delete(int itemId)
        {
            var item = await _database.Items.SingleOrDefaultAsync(i => i.Id == itemId);
            if (item == null)
            {
                return new ServiceResult<bool>().NotFound();
            }

            _database.Items.Remove(item);
            await _database.SaveChangesAsync();

            return new ServiceResult<bool>(true)
            {
                Messages = new List<ServiceMessage>
        {
            new ServiceMessage { Code = "Deleted", Message = "Item deleted successfully" }
        }
            };
        }
    }
}
