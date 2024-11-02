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
    public class NegativeGameEventService : INegativeGameEventService
    {
        private readonly ActionCommandGameDbContext _database;

        public NegativeGameEventService(ActionCommandGameDbContext database)
        {
            _database = database;
        }

        public async Task<ServiceResult<NegativeGameEventResultDto>> GetRandomNegativeGameEvent()
        {
            var gameEventsResult = await Find();
            if (gameEventsResult.Data == null || !gameEventsResult.Data.Any())
            {
                return new ServiceResult<NegativeGameEventResultDto>
                {
                    Messages = new List<ServiceMessage>
                    {
                        new ServiceMessage { Code = "NotFound", Message = "No negative game events found." }
                    }
                };
            }

            // Get a random NegativeGameEvent DTO
            var randomEvent = GameEventHelper.GetRandomNegativeGameEvent(gameEventsResult.Data);
            if (randomEvent == null)
            {
                return new ServiceResult<NegativeGameEventResultDto>
                {
                    Messages = new List<ServiceMessage>
                    {
                        new ServiceMessage { Code = "NoEvent", Message = "No random negative game event was selected." }
                    }
                };
            }

            return new ServiceResult<NegativeGameEventResultDto>(randomEvent);
        }

        public async Task<ServiceResult<IList<NegativeGameEventResultDto>>> Find()
        {
            // Fetch the NegativeGameEvent entities from the database
            var negativeGameEvents = await _database.NegativeGameEvents
                .ProjectToResult()
                .ToListAsync();

            return new ServiceResult<IList<NegativeGameEventResultDto>>(negativeGameEvents);
        }

       
    }
}
