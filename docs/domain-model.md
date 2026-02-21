# Domain Model

## User vs Member

### `User` — обліковий запис (автентифікація)

Представляє людину в системі незалежно від сімейного контексту.

| Поле | Опис |
|------|------|
| `Email` / `PasswordHash` | Дані для входу |
| `Role` (`UserRole`) | Системна роль: `Admin`, `Teacher`, `UserParent`, `UserChild` |
| `Credits` | Баланс кредитів |
| `EmailConfirmed` | Підтвердження email |
| `SchoolId` | Прив'язка до школи (для вчителів / school-admin) |
| `FamilyId` | Прив'язка до сім'ї |

### `Member` — участь у сім'ї

Представляє роль конкретного `User` всередині конкретної `Family`.

| Поле | Опис |
|------|------|
| `UserId` | Посилання на обліковий запис |
| `FamilyId` | Посилання на сім'ю |
| `MemberRoles` | Ролі в сім'ї (child, parent, teacher...) |
| `SurveyResults` | Результати опитувань в контексті сім'ї |

### Різниця в двох словах

> `User` = **хто ти в системі**, `Member` = **яку роль ти граєш у сім'ї**

### Схема зв'язків

```
User (auth)
 ├── Role (UserRole enum)  ← системна роль
 └── Member (1:1)
       ├── Family (M:1)
       └── MemberRoles (M:M)
             └── Role  ← child / parent / teacher / school-admin...
```

---

## Roles

Ролі зберігаються в таблиці `Roles` і заповнюються при старті через `DataSeeder`.

| Name | Опис |
|------|------|
| `admin` | Системний адміністратор |
| `student` | Студент |
| `child` | Дитина в сім'ї |
| `parent` | Батько/мати в сім'ї |
| `teacher` | Вчитель |
| `school-admin` | Адміністратор школи |

### Seed-дані

При першому запуску автоматично створюється:
- всі 6 ролей
- адміністратор `admin@careergg.com` / `Admin123!`

Логіка в [`src/CGG.Infrastructure/Data/DataSeeder.cs`](../src/CGG.Infrastructure/Data/DataSeeder.cs), викликається з [`src/CGG.Api/Program.cs`](../src/CGG.Api/Program.cs).

---

## JWT Context Switching

Користувач може мати кілька контекстів одночасно (сім'я + школа). JWT містить **активний контекст**, який визначає, від імені кого діє запит.

### Claims у токені

| Claim | Значення | Приклад |
|-------|----------|---------|
| `sub` | ID користувача | `uuid` |
| `email` | Email | `user@careergg.com` |
| `role` | Системна роль | `UserParent`, `Teacher` |
| `ctx_type` | Тип контексту | `System`, `Family`, `School` |
| `ctx_id` | ID сім'ї або школи | `uuid` |
| `ctx_role` | Роль у контексті | `parent`, `teacher`, `school-admin` |

### Типи контекстів

| `ctx_type` | Коли | `ctx_role` |
|------------|------|------------|
| `System` | Admin, дефолт | роль з `UserRole` |
| `Family` | Користувач є членом сім'ї | `parent`, `child` |
| `School` | Користувач прив'язаний до школи | `teacher`, `school-admin` |

### Логін — відповідь

```json
{
  "token": "...",
  "activeContext": {
    "type": "Family",
    "contextId": "uuid",
    "contextRole": "parent",
    "contextName": "Сім'я Іванових"
  },
  "availableContexts": [
    { "type": "System", "contextRole": "UserParent" },
    { "type": "Family", "contextId": "uuid", "contextRole": "parent", "contextName": "Сім'я Іванових" },
    { "type": "School", "contextId": "uuid", "contextRole": "teacher", "contextName": "Школа №1" }
  ],
  "canSwitchContext": true
}
```

### Прапорець `canSwitchContext`

| Користувач | Non-system контексти | `canSwitchContext` |
|------------|---------------------|-------------------|
| Лише батько | Family → 1 | `false` — кнопки немає |
| Лише вчитель | School → 1 | `false` — кнопки немає |
| Вчитель + батько | Family + School → 2 | **`true`** — кнопка є |
| Admin | — → 0 | `false` — кнопки немає |

> Фронт показує кнопку переключення **тільки** якщо `canSwitchContext === true`.

### Переключення контексту

```http
POST /api/auth/switch-context
Authorization: Bearer <current_token>

{ "contextType": "School", "contextId": "uuid" }
```

Відповідь — новий JWT з оновленим `ctx_type` / `ctx_id` / `ctx_role`:

```json
{
  "token": "новий JWT...",
  "activeContext": {
    "type": "School",
    "contextId": "uuid",
    "contextRole": "teacher",
    "contextName": "Школа №1"
  }
}
```

### Файли реалізації

| Файл | Опис |
|------|------|
| [`SwitchContextDto.cs`](../src/CGG.Application/DTOs/Auth/SwitchContextDto.cs) | DTO для request/response + `UserTokenContext` |
| [`LoginDto.cs`](../src/CGG.Application/DTOs/Auth/LoginDto.cs) | `LoginResponseDto` з `availableContexts`, `canSwitchContext` |
| [`AuthService.cs`](../src/CGG.Application/Services/AuthService.cs) | `GenerateJwtToken(user, context)`, `GetAvailableContextsAsync` |
| [`SwitchContextCommandHandler.cs`](../src/CGG.Application/Features/Auth/Commands/SwitchContext/SwitchContextCommandHandler.cs) | Валідація доступу + видача нового токену |
| [`AuthController.cs`](../src/CGG.Api/Controllers/AuthController.cs) | `POST /api/auth/switch-context` endpoint |
