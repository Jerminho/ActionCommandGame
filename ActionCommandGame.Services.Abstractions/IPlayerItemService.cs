using ActionCommandGame.DTO.Filters;
using ActionCommandGame.DTO.Results;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPlayerItemService
    {
        Task<ServiceResult<PlayerItemResultDto>> Get(int id);
        Task<ServiceResult<IList<PlayerItemResultDto>>> Find(PlayerItemFilterDto filter);
        Task<ServiceResult<PlayerItemResultDto>> Create(int playerId, int itemId);
        Task<ServiceResult> Delete(int id);
    }
}
