# Career Guidance Guild (CGG)

Full-stack application for career guidance surveys with AI-powered recommendations.

## 🚀 Quick Start

```powershell
.\run-local-service.ps1
```

This will start both Backend (API) and Frontend (Angular) services.

## 📋 Prerequisites

- .NET 10.0 SDK
- Node.js 18+ and npm
- SQL Server or SQL Server LocalDB
- PowerShell 7+

## 🏗️ Architecture

```
CGG.Api               - ASP.NET Core Web API
CGG.Application       - Business Logic & Services
CGG.Core              - Domain Entities & Interfaces
CGG.Infrastructure    - Data Access & External Services
CGG.WebClient         - Angular Frontend
```

## 🛠️ Technology Stack

### Backend
- .NET 10.0
- ASP.NET Core Identity
- Entity Framework Core
- JWT Authentication
- AutoMapper
- FluentValidation

### Frontend
- Angular 21.0.0
- TailwindCSS
- Standalone Components
- RxJS

## 📚 Documentation

- [Startup Guide](STARTUP.md) - Detailed startup instructions
- [Dependency Injection](DEPENDENCY_INJECTION.md) - DI architecture
- [Package Management](PACKAGE_MANAGEMENT.md) - Centralized package versioning
- [Migration Guide](MIGRATION_GUIDE.md) - Database migration info
- [Domain Model](docs/domain-model.md) - User vs Member, Roles, seed-дані

## 🎯 Features

- ✅ Multi-language survey system
- ✅ AI-powered career recommendations
- ✅ Family account management
- ✅ Promo code system
- ✅ Transaction tracking
- ✅ School partnerships

## 🔧 Development

### Start All Services
```powershell
.\run-local-service.ps1
```

### Start with Auto-Reload
```powershell
.\start-dev.ps1
```

### Start Individual Services
```powershell
# Backend only
.\start-backend.ps1

# Frontend only
.\start-frontend.ps1
```

## 🌐 URLs

- Frontend: http://localhost:4200
- Backend API: https://localhost:7000
- Swagger UI: https://localhost:7000/swagger

## 📦 Project Structure

```
src/
├── CGG.Api/              # Web API
│   ├── Controllers/      # API Controllers
│   ├── Dependencies.cs   # API DI registration
│   └── Program.cs        # Application entry point
├── CGG.Application/      # Business Logic
│   └── Dependencies.cs   # Application DI registration
├── CGG.Core/             # Domain Layer
│   └── Entities/         # Domain entities
├── CGG.Infrastructure/   # Data & External Services
│   ├── Data/            # DbContext & Configurations
│   └── Dependencies.cs   # Infrastructure DI registration
└── Directory.Build.props # Centralized package versions

CGG.WebClient/            # Angular App
├── src/
│   ├── app/
│   │   ├── core/        # Core services
│   │   ├── features/    # Feature modules
│   │   ├── pages/       # Page components
│   │   └── shared/      # Shared components
│   └── environments/    # Environment configs
└── public/assets/       # Static assets
```

## 🗄️ Database

The application uses Entity Framework Core with SQL Server.

### Connection String
Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CGG;Trusted_Connection=true;"
  }
}
```

### Migrations
```powershell
cd src/CGG.Api
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 🔐 Configuration

### JWT Settings
Update `appsettings.json`:
```json
{
  "JwtSettings": {
    "SecretKey": "your-secret-key-min-32-chars",
    "Issuer": "CGG.Api",
    "Audience": "CGG.WebClient",
    "ExpirationHours": 24
  }
}
```

### CORS
Update `appsettings.json`:
```json
{
  "Cors": {
    "AllowedOrigins": ["http://localhost:4200"]
  }
}
```

## 🧪 Testing

```powershell
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true
```

## 📝 License

[Your License Here]

## 👥 Contributors

[Your Team Here]

## 📧 Support

For issues and questions, please create a GitHub issue.
