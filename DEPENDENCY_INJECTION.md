# Dependency Injection Architecture

This solution uses layered dependency registration through `Dependencies.cs` files in each project.

## Architecture Pattern

Each layer registers its own dependencies through extension methods:

```
CGG.Api/Dependencies.cs
    ↓ calls
CGG.Infrastructure/Dependencies.cs
    ↓ calls  
CGG.Application/Dependencies.cs
```

## Files Structure

### 1. [CGG.Api/Dependencies.cs](src/CGG.Api/Dependencies.cs)
**Purpose**: API-specific services (Authentication, CORS, Identity)

```csharp
services.AddApiServices(configuration)
```

**Registers:**
- ASP.NET Core Identity (User, Roles)
- JWT Authentication
- CORS policies
- API-specific middleware

### 2. [CGG.Infrastructure/Dependencies.cs](src/CGG.Infrastructure/Dependencies.cs)
**Purpose**: Data access and external services

```csharp
services.AddInfrastructure(configuration)
```

**Registers:**
- DbContext (Entity Framework)
- Repositories
- Email services
- External API clients
- Caching providers

### 3. [CGG.Application/Dependencies.cs](src/CGG.Application/Dependencies.cs)
**Purpose**: Business logic and cross-cutting concerns

```csharp
services.AddApplication()
```

**Registers:**
- AutoMapper profiles
- FluentValidation validators
- Application services (SurveyService, AuthService, etc.)
- Domain event handlers

## Usage in Program.cs

```csharp
using CGG.Api;
using CGG.Application;
using CGG.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Core ASP.NET services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Layer dependencies (order matters!)
builder.Services.AddInfrastructure(builder.Configuration);  // 1. Data layer
builder.Services.AddApplication();                          // 2. Business logic
builder.Services.AddApiServices(builder.Configuration);     // 3. API layer

var app = builder.Build();
```

## Adding New Services

### Example: Adding a Survey Service

1. **Create service interface** in `CGG.Application/Interfaces/ISurveyService.cs`
2. **Implement service** in `CGG.Application/Services/SurveyService.cs`
3. **Register in** `CGG.Application/Dependencies.cs`:
   ```csharp
   services.AddScoped<ISurveyService, SurveyService>();
   ```

### Example: Adding a Repository

1. **Create repository interface** in `CGG.Core/Interfaces/ISurveyRepository.cs`
2. **Implement repository** in `CGG.Infrastructure/Repositories/SurveyRepository.cs`
3. **Register in** `CGG.Infrastructure/Dependencies.cs`:
   ```csharp
   services.AddScoped<ISurveyRepository, SurveyRepository>();
   ```

## Benefits

✅ **Clean Program.cs** - All DI logic is in dedicated files  
✅ **Separation of Concerns** - Each layer manages its own dependencies  
✅ **Testability** - Easy to mock dependencies per layer  
✅ **Maintainability** - Changes are localized to relevant layer  
✅ **Discoverability** - Clear where services are registered  

## Layer Dependencies

```
CGG.Api (Web API)
  ├─ CGG.Application (Business Logic)
  │   └─ CGG.Core (Domain Entities)
  └─ CGG.Infrastructure (Data & External Services)
      └─ CGG.Core (Domain Entities)
```

**Rule**: Lower layers never reference upper layers.

## Configuration Order

⚠️ **Important**: Register dependencies in correct order:

1. **Infrastructure** - Database, repositories must be available first
2. **Application** - Business services depend on repositories
3. **API** - Authentication/Identity needs DbContext from Infrastructure

## Testing

Each layer can be tested independently by mocking its dependencies:

```csharp
// Testing Application layer
var mockRepo = new Mock<ISurveyRepository>();
var service = new SurveyService(mockRepo.Object);
```

## References

- [Dependency Injection in .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
