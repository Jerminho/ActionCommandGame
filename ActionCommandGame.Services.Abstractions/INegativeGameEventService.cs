using ActionCommandGame.DTO.Results;
using ActionCommandGame.Services.Model.Core;

namespace ActionCommandGame.Services.Abstractions
{
    public interface INegativeGameEventService
    {
        Task<ServiceResult<NegativeGameEventResultDto>> GetRandomNegativeGameEvent();
    }
}
