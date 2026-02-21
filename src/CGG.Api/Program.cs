using CGG.Api;
using CGG.Application;
using CGG.Core.Interfaces;
using CGG.Infrastructure;
using CGG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Infrastructure layer dependencies (Database, Identity, Repositories)
builder.Services.AddInfrastructure(builder.Configuration);

// Add Application layer dependencies (Services, AutoMapper, Validation)
builder.Services.AddApplication(builder.Configuration);

// Add API layer dependencies (JWT, CORS)
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

// Apply migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        app.Logger.LogInformation("Database migration completed successfully");

        var seeder = new DataSeeder(context, services.GetRequiredService<IPasswordHasher>());
        await seeder.SeedAsync();
        app.Logger.LogInformation("Database seeding completed successfully");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while migrating the database");
        throw;
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
