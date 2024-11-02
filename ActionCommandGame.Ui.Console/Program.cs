using System.Text;
using ActionCommandGame.Configuration;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Ui.ConsoleApp.Navigation;
using ActionCommandGame.Ui.ConsoleApp.Stores;
using ActionCommandGame.Ui.ConsoleApp.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ActionCommandGame.Ui.ConsoleApp
{
    class Program
    {
        private static IServiceProvider? ServiceProvider { get; set; }
        private static IConfiguration? Configuration { get; set; }

        static async Task Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var navigationManager = ServiceProvider.GetRequiredService<NavigationManager>();

            Console.OutputEncoding = Encoding.UTF8;

            await navigationManager.NavigateTo<TitleView>();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            var appSettings = new AppSettings();
            Configuration?.Bind(nameof(AppSettings), appSettings);
            services.AddSingleton(appSettings);

            // Register the SDKs
            var apiUrl = Configuration["ApiUrl"];
            services.AddActionCommandGameSdk(apiUrl);

            // Register other services
            services.AddSingleton<MemoryStore>();
            services.AddTransient<NavigationManager>();
            // Register the Views
            services.AddTransient<ExitView>();
            services.AddTransient<GameView>();
            services.AddTransient<HelpView>();
            services.AddTransient<InventoryView>();
            services.AddTransient<LeaderboardView>();
            services.AddTransient<PlayerSelectionView>();
            services.AddTransient<ShopView>();
            services.AddTransient<TitleView>();
        }
    }
}
