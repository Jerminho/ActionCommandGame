using ActionCommandGame.DTO.Filters;
using ActionCommandGame.DTO.Results;
using ActionCommandGame.Services.Model.Core;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPlayerService
    {
        Task<ServiceResult<PlayerResultDto>> Get(int id);
        Task<ServiceResult<IList<PlayerResultDto>>> Find(PlayerFilterDto? filter);
    }
}
