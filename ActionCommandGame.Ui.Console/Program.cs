using ActionCommandGame.Configuration; 
using ActionCommandGame.Repository;
using ActionCommandGame.Services;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.Navigation;
using ActionCommandGame.Ui.ConsoleApp.Stores;
using ActionCommandGame.Ui.ConsoleApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

class Program
{
    private static IServiceProvider? ServiceProvider { get; set; }
    private static IConfiguration? Configuration { get; set; }

    static async Task Main()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath((Directory.GetCurrentDirectory()))
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

        var connectionString = "Server=NHP-LENOVO\\VIVES;Database=ActionCommandGame;Trusted_Connection=True;TrustServerCertificate=True;";

        // Register the DbContext with the DI container
        services.AddDbContext<ActionCommandGameDbContext>(options =>
        {
            options.UseSqlServer(connectionString); // Use the appropriate database provider
        });

        // Register your services here
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<INegativeGameEventService,NegativeGameEventService>();
        services.AddScoped<IPositiveGameEventService,PositiveGameEventService>();
        services.AddScoped<IPlayerItemService, PlayerItemService>();

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
