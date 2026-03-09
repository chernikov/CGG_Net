# CLAUDE.md - Career Guidance Guild (CGG)

## Project Overview

Full-stack career guidance platform with AI-powered recommendations.
**Stack:** .NET 10 (C#) backend + Angular 21 (TypeScript) frontend.
**Localization:** Ukrainian primary, English, Hindi.

## Architecture

Clean Architecture with 4 backend layers + Angular SPA:

```
CGG.Api             → Controllers, JWT auth, middleware (entry point)
CGG.Application     → MediatR handlers, services, DTOs, FluentValidation, AutoMapper
CGG.Core            → Domain entities, interfaces (zero dependencies)
CGG.Infrastructure  → EF Core repositories, DbContext, external services (AI, Email)
CGG.WebClient       → Angular 21 standalone components, NgRx, TailwindCSS
```

**Dependency flow:** Api → Infrastructure → Application → Core

## Quick Start

```powershell
.\run-local-service.ps1      # Both backend (https://localhost:7070) + frontend (localhost:4200)
.\start-backend.ps1          # Backend only
.\start-frontend.ps1         # Frontend only
```

Manual: `cd src/CGG.Api && dotnet run` | `cd CGG.WebClient && npm start`

## Key Patterns

### Backend

- **CQRS via MediatR**: Commands/Queries in `CGG.Application/Features/{Feature}/Commands|Queries/`
- **Call flow**: `Handler → Service → Repository`. Handler calls `_unitOfWork.SaveChangesAsync()` at the end
- **Repository split**: `IReadRepository<T>` for reads, `IRepository<T>` for writes
- **Specification pattern**: Query descriptors in `CGG.Application/Specifications/`. Never use raw lambda queries outside specs
- **Unit of Work**: Only exposes `SaveChangesAsync` — no repo access, no transactions
- **DI**: Each layer has `Dependencies.cs` with `IServiceCollection` extension methods
- **Package versions**: Centralized in `src/Directory.Packages.props` (CPM)
- **Solution file**: `src/CareerGuidanceGuild.slnx` (modern .slnx format)

### Frontend

- **Standalone components** (no NgModules)
- **NgRx store** per feature: `store/{feature}/` with actions, reducer, effects, selectors, state files
- **Signals**: Use `store.selectSignal(selector)` for reactive state in components
- **Effect conventions**: `switchMap` for reads, `exhaustMap` for writes, `{ dispatch: false }` for navigation-only
- **API proxy**: Dev uses `/api` path proxied to `https://localhost:7070`. Never use absolute URLs in services
- **Styling**: TailwindCSS utility classes + SCSS
- **Component files**: Always use external files — `templateUrl: './{name}.component.html'` and `styleUrl: './{name}.component.scss'`. Never use inline `template:` or `styles:` in `@Component`

## Coding Conventions

### C# (.NET)

- Records for Commands/Queries: `public record RegisterCommand : IRequest<ResponseDto>`
- `required` keyword for mandatory properties
- FluentValidation validators alongside each Command/Query
- AutoMapper profiles in `CGG.Application/Mappings/`
- Never expose entities through API — always use DTOs
- Structured logging: `_logger.LogInformation("User {UserId} registered", user.Id)`
- Null checks with `is null` / `is not null`
- Always pass `CancellationToken` through handlers

### TypeScript (Angular)

- Strict mode enabled
- Services with `@Injectable({ providedIn: 'root' })`
- Effects use `inject()`, not constructor injection
- Clear error on `OnDestroy`: dispatch `clearXxxError` to avoid stale errors

### No Fallback Logic

Fail fast and explicitly. Do not hide problems with default values.
- Required config missing → `throw new InvalidOperationException(...)`
- Entity not found → `throw new NotFoundException(...)`
- Fallbacks OK only for: UI display text, optional features, default pagination

## File Naming

- Backend: PascalCase (`RegisterCommand.cs`, `AuthService.cs`)
- Frontend: kebab-case (`auth.service.ts`, `add-child.component.ts`)
- NgRx files: `{feature}.actions.ts`, `{feature}.reducer.ts`, `{feature}.effects.ts`, `{feature}.selectors.ts`, `{feature}.state.ts`

## Database

- **ORM**: Entity Framework Core (SQL Server)
- **Migrations**: `src/CGG.Infrastructure/Data/Migrations/`
- Auto-migration + auto-seeding on startup
- Entity configs in `CGG.Infrastructure/Data/Configurations/`
- Seed data (JSON) in `CGG.Infrastructure/Data/SeedData/`

## Testing

- Backend: xUnit + Moq + FluentAssertions. Naming: `MethodName_Scenario_ExpectedResult`
- Frontend: Vitest
- Run: `dotnet test` (backend), `npm test` (frontend)

## Git Conventions

Conventional commits: `feat:`, `fix:`, `docs:`, `refactor:`, `test:`, `chore:`

## Key Directories

```
src/CGG.Api/Controllers/              # API endpoints
src/CGG.Application/Features/         # CQRS handlers (Commands + Queries)
src/CGG.Application/DTOs/             # Request/Response DTOs
src/CGG.Application/Specifications/   # Query specifications
src/CGG.Core/Entities/                # Domain models
src/CGG.Core/Interfaces/              # Repository & service contracts
src/CGG.Infrastructure/Repositories/  # Repository implementations
src/CGG.Infrastructure/Data/          # DbContext, migrations, configs, seed data
src/CGG.Infrastructure/Services/      # AI, email, external integrations
CGG.WebClient/src/app/core/           # Angular services, guards, interceptors
CGG.WebClient/src/app/store/          # NgRx state management
CGG.WebClient/src/app/pages/          # Page components
CGG.WebClient/src/app/shared/         # Shared/reusable components
docs/                                 # Domain model, AI models docs
```

## External Integrations

- **AI**: OpenAI API (gpt-5-mini, gpt-5-nano) — config in `appsettings.json` → `OpenAI` section
- **Email**: Resend API + MailKit
- **Payments**: Monobank
- **Auth**: JWT Bearer tokens

## Docker

- Dev: `docker-compose.yml` (SQL Server + API + WebClient)
- Prod: `docker-compose.prod.yml`
- Deploy: `.\build-and-push.ps1` → SSH to server → `./deploy-server.sh`

## Documentation

- [Domain Model](docs/domain-model.md) — entity relationships
- [OpenAI Models](docs/openai-models.md) — model selection rules
- [Startup Guide](STARTUP.md)
- [DI Architecture](DEPENDENCY_INJECTION.md)
- [Package Management](PACKAGE_MANAGEMENT.md)
- [Migration Guide](MIGRATION_GUIDE.md)
