using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ActionCommandGame.DTO.Filters;
using ActionCommandGame.DTO.Requests;
using ActionCommandGame.DTO.Results;
using ActionCommandGame.Services.Model;
using ActionCommandGame.Services.Model.Core;

namespace ActionCommandGame.Sdk
{
    public class PlayerSdk(IHttpClientFactory httpClientFactory)
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        // Find 
        public async Task<IList<PlayerResultDto>> FindAsync(PlayerFilterDto filter)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");

            var route = "player";

            if (filter != null && filter.FilterUserPlayers.HasValue)
            {
                route += $"?filterUserPlayers={filter.FilterUserPlayers.Value}";
            }

            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<IList<PlayerResultDto>>();
            return result ?? new List<PlayerResultDto>();
        }

        // Get by ID
        public async Task<ServiceResult<PlayerResultDto>> GetAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");

            var route = $"player/{id}";
            var response = await httpClient.GetAsync(route);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ServiceResult<PlayerResultDto>>();
            return result ?? new ServiceResult<PlayerResultDto>();
        }

        // Create a new player 
        public async Task<ServiceResult<PlayerResultDto>> CreateAsync(PlayerRequestDto request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");

            var route = "player";
            var response = await httpClient.PostAsJsonAsync(route, request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ServiceResult<PlayerResultDto>>();
            return result ?? new ServiceResult<PlayerResultDto>();
        }

        public async Task<ServiceResult<PlayerResultDto>> UpdateAsync(int id, PlayerRequestDto request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");

            var route = $"player/{id}";
            var response = await httpClient.PutAsJsonAsync(route, request);

            
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ServiceResult<PlayerResultDto>>();

            if (result == null)
            {
                result = new ServiceResult<PlayerResultDto>
                {
                    Messages = new List<ServiceMessage>
            {
                new ServiceMessage
                {
                    Code = "NotFound", 
                    Message = "Player not found.",
                    MessagePriority = MessagePriority.Error 
                }
            }
                };
            }

            if (result.Messages == null || !result.Messages.Any())
            {
                result.Messages = new List<ServiceMessage>();
            }

            return result;
        }

        // Delete player
        public async Task<ServiceResult<PlayerResultDto>> DeleteAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGameApi");

            var route = $"player/{id}";
            var response = await httpClient.DeleteAsync(route);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ServiceResult<PlayerResultDto>>();
            return result ?? new ServiceResult<PlayerResultDto>();
        }
    }
}
