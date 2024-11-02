using Microsoft.Extensions.DependencyInjection;

namespace ActionCommandGame.Sdk.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddActionCommandGameSdk(this IServiceCollection services, string apiUrl)
        {
            // Register the HttpClient with a named client
            services.AddHttpClient("ActionCommandGameApi", client =>
            {
                client.BaseAddress = new Uri(apiUrl);
            });

            // Register your API clients
            services.AddScoped<GameSdk>();
            services.AddScoped<PlayerSdk>();
            services.AddScoped<ItemSdk>();
            services.AddScoped<PlayerItemSdk>();
            services.AddScoped<NegativeGameEventSdk>();
            services.AddScoped<PositiveGameEventSdk>();

            return services;
        }
    }
}
