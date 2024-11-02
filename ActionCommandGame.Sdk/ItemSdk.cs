using ActionCommandGame.DTO.Results;
using ActionCommandGame.Model;
using ActionCommandGame.Services.Model.Core;
using System.Net.Http.Json;

namespace ActionCommandGame.Sdk
{
    public class ItemSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ItemSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Get Item by ID
        public async Task<ServiceResult<ItemResultDto>> GetItemAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var response = await httpClient.GetAsync($"item/{id}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ServiceResult<ItemResultDto>>();
            return result ?? new ServiceResult<ItemResultDto> { Messages = new List<ServiceMessage> { new ServiceMessage { Code = "NotFound", Message = "Item not found.", MessagePriority = MessagePriority.Error } } };
        }

        // Create Item
        public async Task<ServiceResult<ItemResultDto>> CreateItemAsync(Item item)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var response = await httpClient.PostAsJsonAsync("item", item);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ServiceResult<ItemResultDto>>();
            return result ?? new ServiceResult<ItemResultDto> { Messages = new List<ServiceMessage> { new ServiceMessage { Code = "CreationFailed", Message = "Item creation failed.", MessagePriority = MessagePriority.Error } } };
        }

        // Update Item
        public async Task<ServiceResult<ItemResultDto>> UpdateItemAsync(int id, Item item)
        {
            item.Id = id;
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var response = await httpClient.PutAsJsonAsync($"item/{id}", item);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ServiceResult<ItemResultDto>>();
            return result ?? new ServiceResult<ItemResultDto> { Messages = new List<ServiceMessage> { new ServiceMessage { Code = "UpdateFailed", Message = "Item update failed.", MessagePriority = MessagePriority.Error } } };
        }

        // Delete Item
        public async Task<ServiceResult<ItemResultDto>> DeleteItemAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var response = await httpClient.DeleteAsync($"item/{id}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ServiceResult<ItemResultDto>>();
            return result ?? new ServiceResult<ItemResultDto> { Messages = new List<ServiceMessage> { new ServiceMessage { Code = "DeleteFailed", Message = "Item deletion failed.", MessagePriority = MessagePriority.Error } } };
        }

        // Get All Items
        public async Task<IList<ItemResultDto>> GetAllItemsAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var response = await httpClient.GetAsync("item");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IList<ItemResultDto>>();
            return result ?? new List<ItemResultDto>();
        }
    }
}
