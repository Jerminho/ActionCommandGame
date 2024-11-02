using ActionCommandGame.DTO.Results;
using ActionCommandGame.Services.Model.Core;
using System.Net.Http.Json;

namespace ActionCommandGame.Sdk
{
    public class PositiveGameEventSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PositiveGameEventSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Get Random Positive Game Event
        public async Task<ServiceResult<PositiveGameEventResultDto>> GetRandomPositiveGameEventAsync(bool hasAttackItem)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var response = await httpClient.GetAsync($"PositiveGameEvent/random?hasAttackItem={hasAttackItem}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ServiceResult<PositiveGameEventResultDto>>();
            return result ?? new ServiceResult<PositiveGameEventResultDto> { Messages = new List<ServiceMessage> { new ServiceMessage { Code = "NotFound", Message = "No random positive game event found.", MessagePriority = MessagePriority.Error } } };
        }
    }
}
