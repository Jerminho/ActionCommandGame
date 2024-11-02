using ActionCommandGame.DTO.Results;
using ActionCommandGame.Model;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IItemService
    {
        Task<ServiceResult<ItemResultDto>> Get(int id);
        Task<ServiceResult<IList<ItemResultDto>>> Find();
        Task<ServiceResult<ItemResultDto>> Create(Item item);
        Task<ServiceResult<ItemResultDto>> Update(Item item);
        Task<ServiceResult<bool>> Delete(int id);
    }
}
