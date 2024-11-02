using ActionCommandGame.DTO.Results;
using ActionCommandGame.Services.Model.Core;
using System.Net.Http.Json;

namespace ActionCommandGame.Sdk
{
    public class GameSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GameSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Perform Action
        public async Task<ServiceResult<GameResultDto>> PerformActionAsync(int playerId)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var response = await httpClient.PostAsync($"Game/perform-action/{playerId}", null);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ServiceResult<GameResultDto>>();
            return result ?? new ServiceResult<GameResultDto> { Messages = new List<ServiceMessage> { new ServiceMessage { Code = "ActionFailed", Message = "Failed to perform action.", MessagePriority = MessagePriority.Error } } };
        }

        // Buy Item
        public async Task<ServiceResult<BuyResultDto>> BuyAsync(int playerId, int itemId)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var response = await httpClient.PostAsync($"Game/buy/{playerId}/{itemId}", null);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ServiceResult<BuyResultDto>>();
            return result ?? new ServiceResult<BuyResultDto> { Messages = new List<ServiceMessage> { new ServiceMessage { Code = "PurchaseFailed", Message = "Failed to buy item.", MessagePriority = MessagePriority.Error } } };
        }
    }
}
