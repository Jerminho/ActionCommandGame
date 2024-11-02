using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.DTO.Results; // Ensure you have the correct using directive

namespace ActionCommandGame.Services.Abstractions
{
    public interface IGameService
    {
        Task<ServiceResult<BuyResultDto>> Buy(int playerId, int itemId);
        Task<ServiceResult<GameResultDto>> PerformAction(int playerId);
    }
}