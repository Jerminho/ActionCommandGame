using ActionCommandGame.DTO.Results;
using ActionCommandGame.Services.Model.Core;
using System.Net.Http.Json;

namespace ActionCommandGame.Sdk
{
    public class PlayerItemSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PlayerItemSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Get Player Items
        public async Task<ServiceResult<PlayerItemResultDto>> GetPlayerItemAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var response = await httpClient.GetAsync($"PlayerItem/{id}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ServiceResult<PlayerItemResultDto>>();
            return result ?? new ServiceResult<PlayerItemResultDto> { Messages = new List<ServiceMessage> { new ServiceMessage { Code = "NotFound", Message = "Player item not found.", MessagePriority = MessagePriority.Error } } };
        }

        // Create Player Item
        public async Task<ServiceResult<PlayerItemResultDto>> CreatePlayerItemAsync(int playerId, int itemId)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var response = await httpClient.PostAsync($"PlayerItem/create/{playerId}/{itemId}", null);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ServiceResult<PlayerItemResultDto>>();
            return result ?? new ServiceResult<PlayerItemResultDto> { Messages = new List<ServiceMessage> { new ServiceMessage { Code = "CreationFailed", Message = "Player item creation failed.", MessagePriority = MessagePriority.Error } } };
        }

        // Delete Player Item
        public async Task<ServiceResult<PlayerItemResultDto>> DeletePlayerItemAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var response = await httpClient.DeleteAsync($"PlayerItem/delete/{id}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ServiceResult<PlayerItemResultDto>>();
            return result ?? new ServiceResult<PlayerItemResultDto> { Messages = new List<ServiceMessage> { new ServiceMessage { Code = "DeleteFailed", Message = "Player item deletion failed.", MessagePriority = MessagePriority.Error } } };
        }
    }
}
