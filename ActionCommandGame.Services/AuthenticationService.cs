using ActionCommandGame.DTO.Requests;
using ActionCommandGame.DTO.Results;
using ActionCommandGame.Services.Abstractions;
using System.Net.Http.Json;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthenticationService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<AuthenticationResult> Register(UserRegisterRequestDto request)
    {
        var httpClient = _httpClientFactory.CreateClient("MyApi");
        try
        {
            var response = await httpClient.PostAsJsonAsync("Identity/register", request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AuthenticationResult>();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error registering user: {ex.Message}");
            throw; 
        }
    }

    public async Task<AuthenticationResult> SignIn(UserSignInRequestDto request)
    {
        var httpClient = _httpClientFactory.CreateClient("MyApi");
        try
        {
            var response = await httpClient.PostAsJsonAsync("Identity/sign-in", request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AuthenticationResult>();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error signing in: {ex.Message}");
            throw;
        }
    }
}
