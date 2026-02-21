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
