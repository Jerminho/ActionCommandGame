using Microsoft.EntityFrameworkCore;
using ActionCommandGame.Repository;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext to use SQL Server with the connection string
builder.Services.AddDbContext<ActionCommandGameDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ActionCommandGameDb")));

// Build the app
var app = builder.Build();

// Apply migrations and initialize data if necessary
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ActionCommandGameDbContext>();
    dbContext.Database.Migrate();   // Applies migrations
    dbContext.Initialize();         // Seed data if needed
}

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
