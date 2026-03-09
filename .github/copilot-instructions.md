# GitHub Copilot Instructions — Career Guidance Guild (CGG)

## Stack

- **Backend:** .NET 10 (C#), Clean Architecture, MediatR CQRS, EF Core, SQL Server
- **Frontend:** Angular 21, standalone components, NgRx, TailwindCSS, Signals
- **Auth:** JWT Bearer
- **AI:** OpenAI API (gpt-5-mini, gpt-5-nano)

## Architecture

```
CGG.Api             → Controllers, JWT auth, middleware
CGG.Application     → MediatR handlers, DTOs, FluentValidation, AutoMapper
CGG.Core            → Domain entities, interfaces (zero dependencies)
CGG.Infrastructure  → EF Core, repositories, AI/Email services
CGG.WebClient       → Angular 21 SPA
```

Dependency flow: `Api → Infrastructure → Application → Core`

## OpenAI Models

> Full reference: `docs/openai-models.md`

| Config key      | Model       | Use case in CGG                          | Input $/1M | Output $/1M |
|-----------------|-------------|------------------------------------------|------------|-------------|
| `NanoModel`     | gpt-5-nano  | Intermediate step analysis (short format)| $0.05      | $0.40       |
| `MiniModel`     | gpt-5-mini  | Final step analysis (full format)        | $0.25      | $2.00       |

Config path: `appsettings.json` → `OpenAI:MiniModel` / `OpenAI:NanoModel`

**Rules:**
- Non-final steps → `NanoModel` (gpt-5-nano) — short format `{ matches: [{ title, matchPercentage }] }`
- Final step → `MiniModel` (gpt-5-mini) — full format with `overallPersonalityProfile`, salary, etc.
- Always pass `response_format: { type: "json_object" }` + system prompt must mention "JSON"
- **Do NOT pass `temperature` or `top_p`** — gpt-5-mini/nano only support default values (unsupported_value error)

## Key Patterns

### Backend

- CQRS: Commands/Queries in `CGG.Application/Features/{Feature}/Commands|Queries/`
- Call flow: `Handler → Service → Repository` → `_unitOfWork.SaveChangesAsync()` in handler
- Specifications for ALL queries — no raw lambdas outside specs
- DTOs only in API — never expose entities
- Fail fast: `throw new NotFoundException(...)`, `throw new InvalidOperationException(...)`
- `required` keyword for mandatory C# properties
- Structured logging: `_logger.LogInformation("User {UserId} did X", userId)`

### Frontend

- NgRx effects: `switchMap` for reads, `exhaustMap` for writes
- API proxy: always use `/api/...` — never absolute URLs
- On `OnDestroy`: dispatch `clearXxxError` to clear stale NgRx errors
- Signals: `store.selectSignal(selector)` for reactive state

## File Naming

- Backend: `PascalCase.cs`
- Frontend: `kebab-case.ts`, NgRx: `{feature}.actions.ts`, `.reducer.ts`, `.effects.ts`, `.selectors.ts`, `.state.ts`

## Migrations

```bash
cd src/CGG.Infrastructure
dotnet ef migrations add <Name> --startup-project ../CGG.Api
```

## Git

Conventional commits: `feat:`, `fix:`, `docs:`, `refactor:`, `test:`, `chore:`
