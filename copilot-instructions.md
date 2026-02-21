# Copilot Instructions - Career Guidance Guild (CGG)

## Project Overview

Full-stack application for career guidance surveys with AI-powered recommendations. Migrated from Next.js/Firebase to .NET/Angular architecture using Clean Architecture principles.

**Primary Languages:** C# (.NET 10), TypeScript (Angular 21)
**Localization:** Ukrainian (email templates, some UI elements)

## Architecture

### Clean Architecture Layers

```
CGG.Core            - Domain entities, interfaces (no dependencies)
CGG.Application     - Business logic, DTOs, services, MediatR handlers
CGG.Infrastructure  - Data access, repositories, external services
CGG.Api             - Web API controllers, middleware, authentication
CGG.WebClient       - Angular frontend (standalone components, NgRx)
```

**Dependency Flow:** `CGG.Api` → `CGG.Infrastructure` → `CGG.Application` → `CGG.Core`

### Key Patterns

- **CQRS with MediatR**: Commands and Queries separated in `CGG.Application/Features/`
- **Repository Pattern**: Generic + specific repositories in `CGG.Infrastructure/Repositories/`
- **Unit of Work**: Transaction management in `CGG.Infrastructure/UnitOfWork.cs`
- **Dependency Injection**: Each layer has `Dependencies.cs` with extension methods

## Backend (.NET)

### Project Structure

```
CGG.Application/
  Features/
    [FeatureName]/
      Commands/
        [Action]Command.cs          # MediatR IRequest<T>
        [Action]CommandHandler.cs   # IRequestHandler<TRequest, TResponse>
        [Action]CommandValidator.cs # FluentValidation AbstractValidator<T>
      Queries/
        [Query]Query.cs
        [Query]QueryHandler.cs
        [Query]QueryValidator.cs
  DTOs/
    [Feature]/
      [Action]RequestDto.cs
      [Action]ResponseDto.cs
  Interfaces/
    I[Service]Service.cs
  Services/
    [Service]Service.cs
  Mappings/
    [Feature]MappingProfile.cs    # AutoMapper profiles
```

### Coding Conventions

#### 1. MediatR Commands & Queries

```csharp
// Command (modifies state)
public record RegisterCommand : IRequest<RegisterResponseDto>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

// Query (reads state)  
public record GetUserByIdQuery(string UserId) : IRequest<UserDto>;

// Handler
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public async Task<RegisterResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Implementation
    }
}
```

#### 2. FluentValidation

```csharp
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
            
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"[A-Z]").WithMessage("Must contain uppercase")
            .Matches(@"[a-z]").WithMessage("Must contain lowercase")
            .Matches(@"\d").WithMessage("Must contain digit");
    }
}
```

#### 3. Controller Pattern

```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RegisterResponseDto>> Register(
        [FromBody] RegisterRequestDto request)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Register), result);
    }
}
```

#### 4. Repository Pattern

```csharp
// Generic repository in CGG.Core
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(string id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(string id);
}

// Specific repository
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}
```

#### 5. Service Pattern

```csharp
public interface IEmailService
{
    Task SendWelcomeEmailAsync(string to, string userName);
    Task SendVerificationEmailAsync(string to, string code);
}

public class EmailService : IEmailService
{
    private readonly IResend _resendClient;
    
    public async Task SendWelcomeEmailAsync(string to, string userName)
    {
        // HTML template with Ukrainian localization
        var htmlContent = $@"
            <div style=""font-family: Arial, sans-serif;"">
                <h1>Вітаємо, {userName}!</h1>
                <p>Дякуємо за реєстрацію в Career Guidance Guild.</p>
            </div>";
        
        var message = new EmailMessage
        {
            From = "noreply@careerguidanceguild.com",
            To = to,
            Subject = "Ласкаво просимо до CGG!",
            HtmlBody = htmlContent
        };
        
        await _resendClient.EmailSendAsync(message);
    }
}
```

### Dependency Injection Registration

Each layer registers services in `Dependencies.cs`:

```csharp
// CGG.Application/Dependencies.cs
public static IServiceCollection AddApplication(
    this IServiceCollection services, 
    IConfiguration configuration)
{
    services.AddAutoMapper(typeof(Dependencies).Assembly);
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Dependencies).Assembly));
    services.AddValidatorsFromAssembly(typeof(Dependencies).Assembly);
    services.AddScoped<IAuthService, AuthService>();
    return services;
}

// CGG.Api/Program.cs
builder.Services
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddApiServices(builder.Configuration);
```

### Configuration

- **appsettings.json**: Production settings (no secrets)
- **appsettings.Development.json**: Development overrides
- **User Secrets**: Sensitive data (API keys, connection strings)

```json
{
  "Resend": {
    "ApiKey": "re_xxx" // Store in user secrets
  },
  "Jwt": {
    "Key": "xxx",
    "Issuer": "CGG.Api",
    "Audience": "CGG.WebClient",
    "ExpiryInMinutes": 60
  },
  "OpenAI": {
    "ApiKey": "sk-xxx", // Store in user secrets
    "MiniModel": "gpt-5-mini", // Faster, cost-efficient GPT-5 for well-defined tasks
    "NanoModel": "gpt-5-nano"  // Fastest, most cost-efficient GPT-5
  }
```

Правила вибору моделі та характеристики: [docs/openai-models.md](docs/openai-models.md)
}
```

## Frontend (Angular)

### Project Structure

```
CGG.WebClient/src/app/
  core/
    models/          # TypeScript interfaces/types
    services/        # API services, auth service
    guards/          # Route guards
    interceptors/    # HTTP interceptors
  features/
    [feature]/
      components/    # Feature-specific components
      [feature].routes.ts
  shared/
    components/      # Reusable components
    directives/
    pipes/
  store/
    auth/
      auth.actions.ts
      auth.reducer.ts
      auth.effects.ts
      auth.selectors.ts
    [feature]/
      ...
```

### NgRx State Management

```typescript
// Actions
export const register = createAction(
  '[Auth] Register',
  props<{ email: string; password: string; name: string }>()
);

export const registerSuccess = createAction(
  '[Auth] Register Success',
  props<{ user: User; token: string }>()
);

export const registerFailure = createAction(
  '[Auth] Register Failure',
  props<{ error: string }>()
);

// Reducer
export const authReducer = createReducer(
  initialState,
  on(register, (state) => ({ ...state, loading: true, error: null })),
  on(registerSuccess, (state, { user, token }) => ({
    ...state,
    user,
    token,
    isAuthenticated: true,
    loading: false
  })),
  on(registerFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  }))
);

// Effects
@Injectable()
export class AuthEffects {
  register$ = createEffect(() =>
    this.actions$.pipe(
      ofType(register),
      switchMap(({ email, password, name }) =>
        this.authService.register(email, password, name).pipe(
          map(response => registerSuccess({ 
            user: response.user, 
            token: response.token 
          })),
          catchError(error => of(registerFailure({ error: error.message })))
        )
      )
    )
  );
}
```

### Angular Conventions

- **Standalone Components**: All components use `standalone: true`
- **Signals**: Prefer signals for reactive state in components
- **TailwindCSS**: Use utility classes for styling
- **TypeScript Strict Mode**: Enabled
- **File Naming**: `feature-name.component.ts`, `feature-name.service.ts`

### API Service Pattern

```typescript
@Injectable({ providedIn: 'root' })
export class ApiService {
  private apiUrl = environment.apiUrl; // '/api' (uses proxy in dev)
  
  constructor(private http: HttpClient) {}
  
  post<T>(endpoint: string, data: any): Observable<T> {
    return this.http.post<T>(`${this.apiUrl}/${endpoint}`, data, {
      headers: this.getHeaders()
    });
  }
  
  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': token ? `Bearer ${token}` : ''
    });
  }
}
```

### Development Proxy Configuration

**IMPORTANT**: In development, use relative API paths (`/api`) that route through the Angular dev server proxy.

**proxy.conf.json**:
```json
{
  "/api": {
    "target": "https://localhost:7070",
    "secure": false,
    "changeOrigin": true,
    "logLevel": "debug"
  }
}
```

**Environment Configuration**:
- **Development** (`environment.ts`): `apiUrl: '/api'` → proxied to `https://localhost:7070/api`
- **Production** (`environment.prod.ts`): `apiUrl: '/api'` → served by reverse proxy (nginx/IIS)

**Why Use Proxy**:
- Avoids CORS issues in development
- Matches production URL structure
- Simplifies environment configuration
- Backend runs on `https://localhost:7070` (see `launchSettings.json`)

**Never** use absolute URLs like `https://localhost:7070/api` directly in ApiService calls.

## Database

- **Provider**: Firestore (migrating to SQL Server/PostgreSQL)
- **ORM**: Entity Framework Core
- **Migration Strategy**: See `plans/database-migration-plan.md`

### Entity Conventions

```csharp
public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual ICollection<Survey> Surveys { get; set; } = new List<Survey>();
}
```

## Testing

### Backend Testing

- **Unit Tests**: xUnit + Moq + FluentAssertions
- **Integration Tests**: WebApplicationFactory
- **Test Naming**: `MethodName_Scenario_ExpectedResult`

```csharp
public class RegisterCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var handler = new RegisterCommandHandler(_mockUnitOfWork.Object, _mapper);
        var command = new RegisterCommand { Email = "test@test.com", Password = "Pass123!" };
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("test@test.com");
    }
}
```

### Frontend Testing

- **Unit Tests**: Jasmine + Karma
- **E2E Tests**: Cypress (planned)

## Common Patterns & Best Practices

### 1. Error Handling

```csharp
// Controller level
try
{
    var result = await _mediator.Send(command);
    return Ok(result);
}
catch (ValidationException ex)
{
    return BadRequest(new { errors = ex.Errors });
}
catch (Exception ex)
{
    _logger.LogError(ex, "Error occurred");
    return StatusCode(500, "An error occurred");
}
```

### 2. Async/Await

- Always use `async/await` for I/O operations
- Use `CancellationToken` in handlers
- Use `ConfigureAwait(false)` in library code (not needed in ASP.NET Core)

### 3. Null Safety

- Use nullable reference types (`string?`)
- Use `required` keyword for required properties
- Check for null with `is null` or `is not null`

### 4. Logging

```csharp
_logger.LogInformation("User {UserId} registered successfully", user.Id);
_logger.LogWarning("Failed login attempt for {Email}", email);
_logger.LogError(ex, "Error sending email to {Email}", email);
```

### 5. DTOs vs Entities

- **Never expose entities** directly through API
- Use **DTOs** for all API requests/responses
- Use **AutoMapper** for entity ↔ DTO mapping

### 6. Avoid Fallback Logic

**DO NOT use fallback logic or default values to hide problems.** Instead, fail fast and explicitly.

❌ **Bad - Hiding problems with fallbacks:**

```csharp
// Bad: Silently using empty string if config missing
var apiKey = configuration["Resend:ApiKey"] ?? "";

// Bad: Using default value without validation
var emailFrom = configuration["Email:From"] ?? "noreply@example.com";

// Bad: Catching all exceptions and continuing
try 
{
    await SendEmailAsync(user.Email);
}
catch 
{
    // Silently ignore - user never gets email!
}

// Bad: Fallback to guest user
var user = await _userRepository.GetByIdAsync(userId) ?? new User { Name = "Guest" };
```

✅ **Good - Explicit validation and error handling:**

```csharp
// Good: Fail fast if required configuration missing
var apiKey = configuration["Resend:ApiKey"] 
    ?? throw new InvalidOperationException("Resend:ApiKey is not configured");

// Good: Validate at startup
if (string.IsNullOrEmpty(builder.Configuration["Email:From"]))
{
    throw new InvalidOperationException("Email:From must be configured");
}

// Good: Explicit error handling with logging
try 
{
    await SendEmailAsync(user.Email);
}
catch (Exception ex)
{
    _logger.LogError(ex, "Failed to send email to {Email}", user.Email);
    throw; // Re-throw or return error response
}

// Good: Return null or throw specific exception
var user = await _userRepository.GetByIdAsync(userId) 
    ?? throw new NotFoundException($"User {userId} not found");
```

**Why avoid fallbacks:**
- **Hides bugs**: Problems go unnoticed until production
- **Silent failures**: Operations appear to succeed but don't
- **Debugging nightmare**: Hard to trace why things don't work
- **Security risks**: Defaults might expose sensitive data
- **Data integrity**: Fallbacks can corrupt business logic

**When fallbacks ARE acceptable:**
- UI display text: `user?.Name ?? "Unknown User"` for display only
- Optional features: `var enableCache = config["EnableCache"] == "true"`
- Default pagination: `var pageSize = request.PageSize ?? 10`

**Best practices:**
- Validate required configuration at startup
- Use `required` keyword for mandatory properties
- Throw specific exceptions (`NotFoundException`, `ValidationException`)
- Log all errors with context
- Return explicit error responses from API

## Development Workflow

### Starting the Application

```powershell
# Full stack
.\run-local-service.ps1

# Backend only
.\start-backend.ps1

# Frontend only
.\start-frontend.ps1
```

### HTTP Testing

Use `CGG.Api.http` file with VS Code REST Client extension:

```http
### Register User
POST {{baseUrl}}/api/auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Password123!",
  "name": "John Doe"
}
```

### Git Commit Messages

Follow conventional commits:

```
feat: add user registration endpoint
fix: resolve validation error in login
docs: update API documentation
refactor: extract email service interface
test: add unit tests for auth handler
chore: update dependencies
```

## Configuration & Secrets

### Backend

- `appsettings.json`: Non-sensitive configuration
- User Secrets: `dotnet user-secrets set "Resend:ApiKey" "your-key"`

### Frontend

- `environments/environment.ts`: Development
- `environments/environment.prod.ts`: Production

## Docker Support

```bash
# Development
docker-compose up

# Production
docker-compose -f docker-compose.prod.yml up
```

## Migration Notes

This project was migrated from Next.js/Firebase to .NET/Angular:

- See `MIGRATION_GUIDE.md` for detailed migration documentation
- Original project preserved in `old_project/CGG_MVP/`
- Migration plans in `plans/` directory
- Endpoint parity matrix: `plans/endpoint-parity-matrix.md`

## Package Management

- **Backend**: Centralized package versions in `Directory.Packages.props`
- **Frontend**: Standard `package.json` with locked versions

See `PACKAGE_MANAGEMENT.md` for details.

## Key Technologies Reference

### Backend
- **.NET 10.0**: Latest LTS
- **MediatR**: CQRS pattern implementation
- **FluentValidation**: Input validation
- **AutoMapper**: Object-to-object mapping
- **ASP.NET Core Identity**: Authentication/authorization
- **JWT**: Token-based auth
- **Resend**: Email service provider

### Frontend
- **Angular 21**: Latest stable
- **NgRx**: State management (Redux pattern)
- **RxJS**: Reactive programming
- **TailwindCSS**: Utility-first CSS
- **TypeScript**: Type-safe JavaScript

## Additional Resources

- [Startup Guide](STARTUP.md)
- [Dependency Injection Architecture](DEPENDENCY_INJECTION.md)
- [Package Management](PACKAGE_MANAGEMENT.md)
- [NgRx Auth Guide](CGG.WebClient/NGRX_AUTH_GUIDE.md)
- [OpenAI Models Reference](docs/openai-models.md)

---

**Last Updated:** February 2026
**Maintainer:** Development Team
**Project Status:** Active Development
