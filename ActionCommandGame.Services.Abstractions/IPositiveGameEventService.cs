using ActionCommandGame.DTO.Results;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPositiveGameEventService
    {
        Task<ServiceResult<PositiveGameEventResultDto>> GetRandomPositiveGameEvent(bool hasAttackItem);
    }
}
