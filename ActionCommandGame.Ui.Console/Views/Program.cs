using ActionCommandGame.Configuration; 
using ActionCommandGame.Repository;
using ActionCommandGame.Services;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.Navigation;
using ActionCommandGame.Ui.ConsoleApp.Stores;
using ActionCommandGame.Ui.ConsoleApp.Views;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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

        var connectionString = Configuration.GetConnectionString("ActionCommandGameDb");

        services.AddHttpClient("MyApi", client =>
        {
            var apiUrl = Configuration["ApiUrl"];
            client.BaseAddress = new Uri(apiUrl);
        });

        services.AddDbContext<ActionCommandGameDbContext>(options =>
        {
            options.UseSqlServer(connectionString); 
        });

        // Configure Identity
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ActionCommandGameDbContext>()
            .AddDefaultTokenProviders();

        // Configure JWT authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            };
        });


        // Register your services here
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<INegativeGameEventService,NegativeGameEventService>();
        services.AddScoped<IPositiveGameEventService,PositiveGameEventService>();
        services.AddScoped<IPlayerItemService, PlayerItemService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();


        services.AddSingleton<MemoryStore>();
        services.AddTransient<NavigationManager>();
       
        services.AddTransient<ExitView>();
        services.AddTransient<GameView>();
        services.AddTransient<HelpView>();
        services.AddTransient<InventoryView>();
        services.AddTransient<LeaderboardView>();
        services.AddTransient<PlayerSelectionView>();
        services.AddTransient<ShopView>();
        services.AddTransient<TitleView>();
        services.AddTransient<LoginView>();
        services.AddTransient<RegisterView>();
    }
}
