using ActionCommandGame.DTO.Filters;
using ActionCommandGame.DTO.Results;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Extensions.Filters;
using ActionCommandGame.Services.Model.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ActionCommandGame.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly ActionCommandGameDbContext _database;

        public PlayerService(ActionCommandGameDbContext database)
        {
            _database = database;
        }

        public async Task<ServiceResult<PlayerResultDto>> Get(int id)
        {
            var player = await _database.Players
                .Where(p => p.Id == id)
                .ProjectToResult()
                .SingleOrDefaultAsync();


            return new ServiceResult<PlayerResultDto>(player);
        }

        public async Task<ServiceResult<IList<PlayerResultDto>>> Find(PlayerFilterDto? filter)
        {
            var players = await _database.Players
                .ApplyFilter(filter)
                .ProjectToResult()
                .ToListAsync();


            return new ServiceResult<IList<PlayerResultDto>>(players);
        }

        
    }
}
