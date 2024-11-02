using ActionCommandGame.DTO.Results;
using ActionCommandGame.Services.Model.Core;
using System.Net.Http.Json;

namespace ActionCommandGame.Sdk
{
    public class NegativeGameEventSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NegativeGameEventSdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Get Random Negative Game Event
        public async Task<ServiceResult<NegativeGameEventResultDto>> GetRandomNegativeGameEventAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");
            var response = await httpClient.GetAsync("NegativeGameEvent/random");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ServiceResult<NegativeGameEventResultDto>>();
            return result ?? new ServiceResult<NegativeGameEventResultDto> { Messages = new List<ServiceMessage> { new ServiceMessage { Code = "NotFound", Message = "No random negative game event found.", MessagePriority = MessagePriority.Error } } };
        }
    }
}
