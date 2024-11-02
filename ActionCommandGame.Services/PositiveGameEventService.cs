using ActionCommandGame.DTO.Results;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Helpers;
using ActionCommandGame.Services.Model.Core;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class PositiveGameEventService : IPositiveGameEventService
    {
        private readonly ActionCommandGameDbContext _database;

        public PositiveGameEventService(ActionCommandGameDbContext database)
        {
            _database = database;
        }

        public async Task<ServiceResult<PositiveGameEventResultDto>> GetRandomPositiveGameEvent(bool hasAttackItem)
        {
            var query = _database.PositiveGameEvents.AsQueryable();

            // If we don't have an attack item, we can only get low-reward items.
            if (!hasAttackItem)
            {
                query = query.Where(p => p.Money < 50);
            }

            var gameEvents = await query
                .ProjectToResult()
                .ToListAsync();


            var randomEvent = GameEventHelper.GetRandomPositiveGameEvent(gameEvents);

            return new ServiceResult<PositiveGameEventResultDto>(randomEvent);
        }

       
    }
}
