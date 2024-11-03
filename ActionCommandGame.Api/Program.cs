using Microsoft.EntityFrameworkCore;
using ActionCommandGame.Repository;
using Microsoft.Extensions.DependencyInjection;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services;

var builder = WebApplication.CreateBuilder(args);

// Hardcoded connection string
var connectionString = "Server=NHP-LENOVO\\VIVES;Database=ActionCommandGame;Trusted_Connection=True;";

// Configure DbContext with connection string
builder.Services.AddDbContext<ActionCommandGameDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

/*using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ActionCommandGameDbContext>();
    dbContext.Database.Migrate();
    dbContext.Initialize();
}*/

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();