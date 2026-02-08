using CGG.Api;
using CGG.Application;
using CGG.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Infrastructure layer dependencies (Database, Identity, Repositories)
builder.Services.AddInfrastructure(builder.Configuration);

// Add Application layer dependencies (Services, AutoMapper, Validation)
builder.Services.AddApplication();

// Add API layer dependencies (JWT, CORS)
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
