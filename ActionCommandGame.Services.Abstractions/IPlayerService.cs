using ActionCommandGame.DTO.Filters;
using ActionCommandGame.DTO.Results;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPlayerService
    {
        Task<ServiceResult<PlayerResultDto>> Get(int id);
        Task<ServiceResult<IList<PlayerResultDto>>> Find(PlayerFilterDto? filter);
    }
}
