# Інструкція з міграції Career Guidance Guild на .NET + MSSQL + Angular

## 📋 Зміст
1. [Огляд поточної архітектури](#огляд-поточної-архітектури)
2. [Нова архітектура](#нова-архітектура)
3. [Підготовка середовища](#підготовка-середовища)
4. [Створення backend (.NET)](#створення-backend-net)
5. [Створення frontend (Angular)](#створення-frontend-angular)
6. [Міграція бази даних](#міграція-бази-даних)
7. [Міграція функціоналу](#міграція-функціоналу)
8. [Тестування та запуск](#тестування-та-запуск)

---

## Огляд поточної архітектури

### Технології
- **Frontend**: Next.js 15 (React) + TypeScript + TailwindCSS
- **Backend**: Next.js API Routes (серверні функції)
- **База даних**: Firebase Firestore (NoSQL)
- **Аутентифікація**: Firebase Auth
- **Email**: Resend API
- **Платежі**: Monobank API
- **AI**: OpenAI GPT API

### Основні функції
1. **Користувачі**: Реєстрація, авторизація, профілі (діти/батьки/вчителі)
2. **Сім'ї**: Групування користувачів з спільним балансом
3. **Опитування (Surveys)**: Інтерактивні опитування для визначення кар'єрних шляхів
4. **AI Рекомендації**: Персоналізовані рекомендації через ChatGPT
5. **Кредитна система**: Баланс користувачів/сімей для оплати послуг
6. **Платежі**: Інтеграція з Monobank
7. **Email сповіщення**: Підтвердження оплати, сповіщення
8. **Адмін панель**: Управління користувачами, школами, промо-кодами

### Структура API endpoints
```
/api/auth/* - Аутентифікація
/api/user/* - Операції з користувачами
/api/family/* - Управління сім'ями
/api/surveys/* - Опитування
/api/ai/* - AI рекомендації
/api/credits/* - Управління кредитами
/api/monobank/* - Платежі
/api/admin/* - Адміністрування
```

---

## Нова архітектура

### Технології
- **Frontend**: Angular 19+ + TypeScript + TailwindCSS
- **Backend**: ASP.NET Core 9.0 Web API + C#
- **База даних**: Microsoft SQL Server 2022
- **ORM**: Entity Framework Core 9.0
- **Аутентифікація**: ASP.NET Identity + JWT
- **Email**: FluentEmail або MailKit
- **Платежі**: Monobank API (той самий)
- **AI**: OpenAI API (той самий)

### Архітектура проєкту
```
CGG_DotNet/
├── src/
│   ├── CGG.Api/                 # ASP.NET Core Web API
│   ├── CGG.Core/                # Domain models, interfaces
│   ├── CGG.Infrastructure/      # EF Core, repositories
│   ├── CGG.Application/         # Business logic, services
│   └── CGG.WebClient/           # Angular application
├── tests/
│   ├── CGG.Api.Tests/
│   ├── CGG.Application.Tests/
│   └── CGG.Infrastructure.Tests/
└── CGG.sln                      # Solution file
```

---

## Підготовка середовища

### Необхідне ПЗ

1. **.NET SDK 9.0**
   ```powershell
   # Встановлення
   winget install Microsoft.DotNet.SDK.9
   
   # Перевірка
   dotnet --version
   ```

2. **Node.js 20+ LTS**
   ```powershell
   # Встановлення
   winget install OpenJS.NodeJS.LTS
   
   # Перевірка
   node --version
   npm --version
   ```

3. **Angular CLI**
   ```powershell
   npm install -g @angular/cli@latest
   
   # Перевірка
   ng version
   ```

4. **SQL Server 2022**
   ```powershell
   # Варіант 1: SQL Server Developer Edition (безкоштовна)
   # Завантажити з https://www.microsoft.com/sql-server/sql-server-downloads
   
   # Варіант 2: SQL Server Express (безкоштовна)
   winget install Microsoft.SQLServer.2022.Express
   
   # Варіант 3: Використовувати Docker
   docker pull mcr.microsoft.com/mssql/server:2022-latest
   ```

5. **SQL Server Management Studio (SSMS)** - опціонально
   ```powershell
   winget install Microsoft.SQLServerManagementStudio
   ```

6. **Visual Studio 2022** або **VS Code**
   ```powershell
   # Visual Studio Community (безкоштовна)
   winget install Microsoft.VisualStudio.2022.Community
   
   # АБО VS Code
   winget install Microsoft.VisualStudioCode
   ```

---

## Створення backend (.NET)

### Крок 1: Ініціалізація проєкту

Створіть нову папку для проєкту (ПОЗА поточною папкою):

```powershell
# Створення нової папки
cd C:\projects\uss
mkdir CGG_DotNet
cd CGG_DotNet

# Створення solution
dotnet new sln -n CareerGuidanceGuild

# Створення проєктів
dotnet new webapi -n CGG.Api -o src/CGG.Api
dotnet new classlib -n CGG.Core -o src/CGG.Core
dotnet new classlib -n CGG.Infrastructure -o src/CGG.Infrastructure
dotnet new classlib -n CGG.Application -o src/CGG.Application

# Додавання проєктів до solution
dotnet sln add src/CGG.Api/CGG.Api.csproj
dotnet sln add src/CGG.Core/CGG.Core.csproj
dotnet sln add src/CGG.Infrastructure/CGG.Infrastructure.csproj
dotnet sln add src/CGG.Application/CGG.Application.csproj

# Встановлення залежностей між проєктами
dotnet add src/CGG.Api/CGG.Api.csproj reference src/CGG.Application/CGG.Application.csproj
dotnet add src/CGG.Api/CGG.Api.csproj reference src/CGG.Infrastructure/CGG.Infrastructure.csproj
dotnet add src/CGG.Application/CGG.Application.csproj reference src/CGG.Core/CGG.Core.csproj
dotnet add src/CGG.Infrastructure/CGG.Infrastructure.csproj reference src/CGG.Core/CGG.Core.csproj
```

### Крок 2: Встановлення NuGet пакетів

```powershell
# Entity Framework Core для SQL Server
dotnet add src/CGG.Infrastructure/CGG.Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer
dotnet add src/CGG.Infrastructure/CGG.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Tools

# Identity для аутентифікації
dotnet add src/CGG.Infrastructure/CGG.Infrastructure.csproj package Microsoft.AspNetCore.Identity.EntityFrameworkCore

# JWT для токенів
dotnet add src/CGG.Api/CGG.Api.csproj package Microsoft.AspNetCore.Authentication.JwtBearer

# Email
dotnet add src/CGG.Infrastructure/CGG.Infrastructure.csproj package MailKit
dotnet add src/CGG.Infrastructure/CGG.Infrastructure.csproj package MimeKit

# HTTP Client для зовнішніх API
dotnet add src/CGG.Infrastructure/CGG.Infrastructure.csproj package Microsoft.Extensions.Http

# Swagger для документації API
dotnet add src/CGG.Api/CGG.Api.csproj package Swashbuckle.AspNetCore

# AutoMapper для маппінгу
dotnet add src/CGG.Application/CGG.Application.csproj package AutoMapper
dotnet add src/CGG.Application/CGG.Application.csproj package AutoMapper.Extensions.Microsoft.DependencyInjection

# Validation
dotnet add src/CGG.Application/CGG.Application.csproj package FluentValidation
dotnet add src/CGG.Application/CGG.Application.csproj package FluentValidation.DependencyInjectionExtensions
```

### Крок 3: Структура моделей даних

Створіть файл `src/CGG.Core/Entities/User.cs`:

```csharp
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CGG.Core.Entities
{
    public enum UserRole
    {
        UserChild,
        UserParent,
        Teacher,
        Admin
    }

    public class User : IdentityUser<Guid>
    {
        public string? DisplayName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public UserRole Role { get; set; }
        public decimal Credits { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Relationships
        public Guid? FamilyId { get; set; }
        public Family? Family { get; set; }
        
        public Guid? MemberId { get; set; }
        public Member? Member { get; set; }
        
        public Guid? SchoolId { get; set; }
        public School? School { get; set; }

        // Collections
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<SurveyResult> SurveyResults { get; set; } = new List<SurveyResult>();
        public ICollection<AIRecommendation> AIRecommendations { get; set; } = new List<AIRecommendation>();
    }

    public class Family
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal Credits { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Collections
        public ICollection<Member> Members { get; set; } = new List<Member>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

    public class Member
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        
        public Guid FamilyId { get; set; }
        public Family Family { get; set; } = null!;
        
        public string Role { get; set; } = "child"; // parent, child
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        
        public Guid? FamilyId { get; set; }
        public Family? Family { get; set; }
        
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty; // payment, promo, admin_add, etc.
        public string? Description { get; set; }
        public string? PaymentId { get; set; }
        public string Status { get; set; } = "completed";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class School
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public decimal Credits { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<User> Users { get; set; } = new List<User>();
    }

    public class SurveyResult
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        
        public string SurveyType { get; set; } = string.Empty;
        public string Answers { get; set; } = "{}"; // JSON
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class AIRecommendation
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        
        public string Content { get; set; } = string.Empty;
        public string? Prompt { get; set; }
        public int TokensUsed { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class PromoCode
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Credits { get; set; }
        public int? MaxUses { get; set; }
        public int UsedCount { get; set; } = 0;
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
```

### Крок 4: DbContext

Створіть файл `src/CGG.Infrastructure/Data/ApplicationDbContext.cs`:

```csharp
using CGG.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CGG.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Family> Families { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<SurveyResult> SurveyResults { get; set; }
        public DbSet<AIRecommendation> AIRecommendations { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // User configuration
            builder.Entity<User>(entity =>
            {
                entity.Property(e => e.Credits).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.Family)
                    .WithMany()
                    .HasForeignKey(e => e.FamilyId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Family configuration
            builder.Entity<Family>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Credits).HasColumnType("decimal(18,2)");
            });

            // Member configuration
            builder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                    .WithOne(u => u.Member)
                    .HasForeignKey<Member>(e => e.UserId);
                entity.HasOne(e => e.Family)
                    .WithMany(f => f.Members)
                    .HasForeignKey(e => e.FamilyId);
            });

            // Transaction configuration
            builder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            });

            // School configuration
            builder.Entity<School>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Credits).HasColumnType("decimal(18,2)");
            });

            // PromoCode configuration
            builder.Entity<PromoCode>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.Credits).HasColumnType("decimal(18,2)");
            });
        }
    }
}
```

### Крок 5: Налаштування API

Створіть файл `src/CGG.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CareerGuidanceGuild;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "JwtSettings": {
    "SecretKey": "YOUR-SECRET-KEY-MINIMUM-32-CHARACTERS-LONG",
    "Issuer": "CareerGuidanceGuild",
    "Audience": "CareerGuidanceGuildUsers",
    "ExpirationInMinutes": 1440
  },
  "OpenAI": {
    "ApiKey": "YOUR-OPENAI-API-KEY",
    "Model": "gpt-4"
  },
  "Email": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "Username": "your-email@gmail.com",
    "Password": "your-app-password",
    "FromEmail": "noreply@careerguide.com",
    "FromName": "Career Guidance Guild"
  },
  "Monobank": {
    "Token": "YOUR-MONOBANK-TOKEN",
    "WebhookUrl": "https://yourdomain.com/api/monobank/webhook"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Cors": {
    "AllowedOrigins": ["http://localhost:4200"]
  }
}
```

### Крок 6: Program.cs

Оновіть `src/CGG.Api/Program.cs`:

```csharp
using CGG.Core.Entities;
using CGG.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>())
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add your services here
// builder.Services.AddScoped<IUserService, UserService>();

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
```

### Крок 7: Міграції бази даних

```powershell
# Встановити EF Core CLI tools (якщо ще не встановлені)
dotnet tool install --global dotnet-ef

# Створити першу міграцію
cd src\CGG.Api
dotnet ef migrations add InitialCreate --project ..\CGG.Infrastructure\CGG.Infrastructure.csproj --startup-project CGG.Api.csproj

# Застосувати міграцію до бази даних
dotnet ef database update --project ..\CGG.Infrastructure\CGG.Infrastructure.csproj --startup-project CGG.Api.csproj
```

### Крок 8: Приклад контролера

Створіть `src/CGG.Api/Controllers/AuthController.cs`:

```csharp
using CGG.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CGG.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                DisplayName = request.DisplayName,
                Role = request.Role,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "User registered successfully", userId = user.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.Email,
                    user.DisplayName,
                    user.Role,
                    user.Credits
                }
            });
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpirationInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
```

---

## Створення frontend (Angular)

### Крок 1: Створення Angular проєкту

```powershell
# Повернутися до кореневої папки
cd C:\projects\uss\CGG_DotNet

# Створити Angular проєкт
ng new CGG.WebClient --routing --style=scss --ssr=false

# Перейти в папку проєкту
cd CGG.WebClient
```

Під час створення оберіть:
- Routing: **Yes**
- Stylesheet: **SCSS**
- SSR: **No**

### Крок 2: Встановлення залежностей

```powershell
# TailwindCSS
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init

# Angular Material (опціонально, якщо потрібен)
ng add @angular/material

# HTTP Client та інші
npm install rxjs

# Для анімацій (аналог Framer Motion)
npm install @angular/animations
```

### Крок 3: Налаштування TailwindCSS

Оновіть `tailwind.config.js`:

```javascript
/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          DEFAULT: '#6366f1',
          dark: '#4f46e5',
        },
        secondary: {
          DEFAULT: '#ec4899',
          dark: '#db2777',
        },
      },
    },
  },
  plugins: [],
}
```

Оновіть `src/styles.scss`:

```scss
@tailwind base;
@tailwind components;
@tailwind utilities;

/* Ваші глобальні стилі */
```

### Крок 4: Структура Angular проєкту

```powershell
# Створення модулів і компонентів
ng generate module core
ng generate module shared
ng generate module features/auth --routing
ng generate module features/surveys --routing
ng generate module features/profile --routing
ng generate module features/admin --routing

# Компоненти аутентифікації
ng generate component features/auth/login
ng generate component features/auth/register
ng generate component features/auth/reset-password

# Сервіси
ng generate service core/services/auth
ng generate service core/services/api
ng generate service core/services/user
ng generate service core/services/survey

# Guards
ng generate guard core/guards/auth

# Interceptors
ng generate interceptor core/interceptors/jwt
```

### Крок 5: Налаштування environment

Створіть `src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7001/api',
  openAiModel: 'gpt-4',
};
```

Створіть `src/environments/environment.prod.ts`:

```typescript
export const environment = {
  production: true,
  apiUrl: 'https://your-production-api.com/api',
  openAiModel: 'gpt-4',
};
```

### Крок 6: API Service

Створіть `src/app/core/services/api.service.ts`:

```typescript
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': token ? `Bearer ${token}` : ''
    });
  }

  get<T>(endpoint: string): Observable<T> {
    return this.http.get<T>(`${this.apiUrl}/${endpoint}`, {
      headers: this.getHeaders()
    });
  }

  post<T>(endpoint: string, data: any): Observable<T> {
    return this.http.post<T>(`${this.apiUrl}/${endpoint}`, data, {
      headers: this.getHeaders()
    });
  }

  put<T>(endpoint: string, data: any): Observable<T> {
    return this.http.put<T>(`${this.apiUrl}/${endpoint}`, data, {
      headers: this.getHeaders()
    });
  }

  delete<T>(endpoint: string): Observable<T> {
    return this.http.delete<T>(`${this.apiUrl}/${endpoint}`, {
      headers: this.getHeaders()
    });
  }
}
```

### Крок 7: Auth Service

Створіть `src/app/core/services/auth.service.ts`:

```typescript
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { ApiService } from './api.service';

interface User {
  id: string;
  email: string;
  displayName: string;
  role: string;
  credits: number;
}

interface LoginResponse {
  token: string;
  user: User;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private api: ApiService,
    private router: Router
  ) {
    this.loadUserFromStorage();
  }

  private loadUserFromStorage(): void {
    const userJson = localStorage.getItem('currentUser');
    if (userJson) {
      try {
        const user = JSON.parse(userJson);
        this.currentUserSubject.next(user);
      } catch (e) {
        console.error('Failed to parse user from storage', e);
      }
    }
  }

  login(email: string, password: string): Observable<LoginResponse> {
    return this.api.post<LoginResponse>('auth/login', { email, password })
      .pipe(
        tap(response => {
          localStorage.setItem('token', response.token);
          localStorage.setItem('currentUser', JSON.stringify(response.user));
          this.currentUserSubject.next(response.user);
        })
      );
  }

  register(email: string, password: string, displayName: string, role: string): Observable<any> {
    return this.api.post('auth/register', { email, password, displayName, role });
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
    this.router.navigate(['/auth/login']);
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }

  getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }
}
```

### Крок 8: Auth Guard

Створіть `src/app/core/guards/auth.guard.ts`:

```typescript
import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.authService.isAuthenticated()) {
      return true;
    }

    this.router.navigate(['/auth/login'], { queryParams: { returnUrl: state.url } });
    return false;
  }
}
```

### Крок 9: Приклад компонента Login

Створіть `src/app/features/auth/login/login.component.ts`:

```typescript
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm: FormGroup;
  loading = false;
  error = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    this.error = '';

    const { email, password } = this.loginForm.value;

    this.authService.login(email, password).subscribe({
      next: () => {
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.error = err.error?.message || 'Login failed';
        this.loading = false;
      },
      complete: () => {
        this.loading = false;
      }
    });
  }
}
```

### Крок 10: Routing

Оновіть `src/app/app-routing.module.ts`:

```typescript
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  {
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: 'surveys',
    loadChildren: () => import('./features/surveys/surveys.module').then(m => m.SurveysModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'profile',
    loadChildren: () => import('./features/profile/profile.module').then(m => m.ProfileModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'admin',
    loadChildren: () => import('./features/admin/admin.module').then(m => m.AdminModule),
    canActivate: [AuthGuard]
  },
  { path: '**', redirectTo: '/home' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
```

---

## Міграція бази даних

### Скрипт експорту з Firestore (Node.js)

Створіть файл `migration/export-firestore.js` в СТАРОМУ проєкті:

```javascript
const admin = require('firebase-admin');
const fs = require('fs');
const path = require('path');

// Ініціалізація Firebase Admin
const serviceAccount = require('../client/service_account.json');

admin.initializeApp({
  credential: admin.credential.cert(serviceAccount)
});

const db = admin.firestore();

async function exportCollection(collectionName) {
  console.log(`Exporting ${collectionName}...`);
  const snapshot = await db.collection(collectionName).get();
  
  const data = [];
  snapshot.forEach(doc => {
    data.push({
      id: doc.id,
      ...doc.data()
    });
  });

  const outputPath = path.join(__dirname, 'output', `${collectionName}.json`);
  fs.writeFileSync(outputPath, JSON.stringify(data, null, 2));
  console.log(`Exported ${data.length} documents from ${collectionName}`);
  
  return data;
}

async function exportAll() {
  // Створення папки output
  const outputDir = path.join(__dirname, 'output');
  if (!fs.existsSync(outputDir)) {
    fs.mkdirSync(outputDir, { recursive: true });
  }

  const collections = [
    'users',
    'families',
    'members',
    'transactions',
    'schools',
    'surveyResults',
    'aiRecommendations',
    'promoCodes'
  ];

  for (const collection of collections) {
    try {
      await exportCollection(collection);
    } catch (error) {
      console.error(`Error exporting ${collection}:`, error);
    }
  }

  console.log('Export completed!');
  process.exit(0);
}

exportAll();
```

Запустіть експорт:

```powershell
cd C:\projects\uss\CGG_Mvp
mkdir migration
cd migration
npm init -y
npm install firebase-admin
node export-firestore.js
```

### Скрипт імпорту в SQL Server (C#)

Створіть консольний додаток для імпорту:

```powershell
cd C:\projects\uss\CGG_DotNet
dotnet new console -n CGG.DataMigration
dotnet add CGG.DataMigration/CGG.DataMigration.csproj reference src/CGG.Infrastructure/CGG.Infrastructure.csproj
dotnet add CGG.DataMigration/CGG.DataMigration.csproj package Newtonsoft.Json
```

Створіть `CGG.DataMigration/Program.cs`:

```csharp
using CGG.Core.Entities;
using CGG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

using var context = new ApplicationDbContext(optionsBuilder.Options);

// Імпорт користувачів
Console.WriteLine("Importing users...");
var usersJson = File.ReadAllText("../migration/output/users.json");
var firestoreUsers = JsonConvert.DeserializeObject<List<FirestoreUser>>(usersJson);

foreach (var fu in firestoreUsers)
{
    var user = new User
    {
        Id = Guid.Parse(fu.uid),
        Email = fu.email,
        UserName = fu.email,
        DisplayName = fu.displayName,
        Credits = fu.credits,
        Role = ParseRole(fu.role),
        EmailConfirmed = fu.emailVerified ?? false,
        CreatedAt = fu.createdAt.ToDateTime(),
        UpdatedAt = fu.updatedAt?.ToDateTime() ?? fu.createdAt.ToDateTime()
    };

    context.Users.Add(user);
}

await context.SaveChangesAsync();
Console.WriteLine($"Imported {firestoreUsers.Count} users");

// Додайте імпорт інших колекцій...

Console.WriteLine("Migration completed!");

// Helper classes
class FirestoreUser
{
    public string uid { get; set; }
    public string email { get; set; }
    public string displayName { get; set; }
    public decimal credits { get; set; }
    public string role { get; set; }
    public bool? emailVerified { get; set; }
    public FirestoreTimestamp createdAt { get; set; }
    public FirestoreTimestamp? updatedAt { get; set; }
}

class FirestoreTimestamp
{
    public long _seconds { get; set; }
    public long _nanoseconds { get; set; }
    
    public DateTime ToDateTime()
    {
        return DateTimeOffset.FromUnixTimeSeconds(_seconds).DateTime;
    }
}

static UserRole ParseRole(string role)
{
    return role switch
    {
        "user_child" => UserRole.UserChild,
        "user_parent" => UserRole.UserParent,
        "teacher" => UserRole.Teacher,
        "admin" => UserRole.Admin,
        _ => UserRole.UserChild
    };
}
```

---

## Міграція функціоналу

### Основні зміни

| Поточний стек | Новий стек | Що змінюється |
|---------------|------------|---------------|
| Next.js API Routes | ASP.NET Core Web API | Переписати всі endpoints як контролери |
| Firebase Auth | ASP.NET Identity + JWT | Нова система аутентифікації |
| Firestore | SQL Server + EF Core | Міграція даних і запитів |
| React Components | Angular Components | Переписати UI компоненти |
| React Hooks | Angular Services/RxJS | Стан та логіка через сервіси |
| Framer Motion | Angular Animations | Анімації через Angular API |
| i18next | @ngx-translate | Інтернаціоналізація |

### Приоритет міграції функцій

1. **Критично важливі** (тиждень 1-2):
   - Аутентифікація (реєстрація, вхід, вихід)
   - Управління користувачами
   - Базові CRUD операції

2. **Важливі** (тиждень 3-4):
   - Опитування (surveys)
   - Кредитна система
   - Сімейні групи

3. **Додаткові** (тиждень 5-6):
   - AI рекомендації
   - Платежі (Monobank)
   - Email сповіщення
   - Адмін панель

---

## Тестування та запуск

### Запуск SQL Server (якщо використовуєте Docker)

```powershell
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" `
  -p 1433:1433 --name sql-server `
  -d mcr.microsoft.com/mssql/server:2022-latest
```

### Запуск Backend (.NET)

```powershell
cd C:\projects\uss\CGG_DotNet\src\CGG.Api
dotnet run
```

API буде доступне на `https://localhost:7001`

### Запуск Frontend (Angular)

```powershell
cd C:\projects\uss\CGG_DotNet\CGG.WebClient
ng serve
```

Angular застосунок буде доступний на `http://localhost:4200`

### Перевірка

1. Відкрийте браузер: `http://localhost:4200`
2. Swagger документація API: `https://localhost:7001/swagger`
3. Спробуйте зареєструватися та увійти

---

## Додаткові ресурси

### Документація
- [ASP.NET Core](https://docs.microsoft.com/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Angular](https://angular.io/docs)
- [SQL Server](https://docs.microsoft.com/sql/sql-server/)

### Корисні команди

```powershell
# .NET
dotnet build                    # Збірка проєкту
dotnet run                      # Запуск
dotnet test                     # Тести
dotnet ef migrations add Name   # Нова міграція
dotnet ef database update       # Оновити БД

# Angular
ng serve                        # Dev server
ng build                        # Production build
ng test                         # Unit tests
ng generate component Name      # Новий компонент
```

### Структура фінального проєкту

```
C:\projects\uss\CGG_DotNet\
├── src\
│   ├── CGG.Api\                    # Web API
│   │   ├── Controllers\
│   │   ├── appsettings.json
│   │   └── Program.cs
│   ├── CGG.Application\            # Business logic
│   │   ├── Services\
│   │   ├── DTOs\
│   │   └── Validators\
│   ├── CGG.Core\                   # Domain
│   │   ├── Entities\
│   │   └── Interfaces\
│   └── CGG.Infrastructure\         # Data access
│       ├── Data\
│       ├── Repositories\
│       └── Services\
├── CGG.WebClient\                  # Angular app
│   ├── src\
│   │   ├── app\
│   │   │   ├── core\
│   │   │   ├── shared\
│   │   │   └── features\
│   │   └── environments\
│   └── angular.json
├── migration\                      # Migration scripts
│   ├── export-firestore.js
│   └── output\
├── tests\                          # Tests
└── CareerGuidanceGuild.sln        # Solution file
```

---

## Контрольний список міграції

- [ ] Встановлено всі необхідні інструменти (.NET, Node.js, Angular CLI, SQL Server)
- [ ] Створено структуру .NET проєкту
- [ ] Налаштовано Entity Framework та створено моделі
- [ ] Застосовано міграції бази даних
- [ ] Створено Angular проєкт
- [ ] Налаштовано TailwindCSS
- [ ] Створено базові сервіси (Auth, API)
- [ ] Експортовано дані з Firestore
- [ ] Імпортовано дані в SQL Server
- [ ] Реалізовано аутентифікацію
- [ ] Міг ровано основні функції
- [ ] Протестовано критичні сценарії
- [ ] Налаштовано production build

---

## Підтримка та питання

Якщо виникнуть питання під час міграції:

1. Перевірте логи у консолі браузера (F12)
2. Перевірте логи .NET API
3. Перегляньте SQL запити через SQL Server Profiler
4. Використовуйте Swagger для тестування API endpoints

Успішної міграції! 🚀
