# NgRx Store Structure

## Файли Auth Store

### 1. **auth.state.ts** - Визначення стану
```typescript
interface AuthState {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  loading: boolean;
  error: string | null;
}
```

### 2. **auth.actions.ts** - Actions
- `login` - початок входу
- `loginSuccess` - успішний вхід
- `loginFailure` - помилка входу
- `register` - реєстрація
- `logout` - вихід
- `loadUserFromStorage` - завантаження з localStorage

### 3. **auth.reducer.ts** - Reducer
Обробка всіх actions та оновлення стану

### 4. **auth.effects.ts** - Side Effects
- HTTP запити до API
- Навігація після успішної операції
- Збереження до localStorage

### 5. **auth.selectors.ts** - Selectors
- `selectUser` - поточний користувач
- `selectIsAuthenticated` - чи авторизований
- `selectAuthLoading` - стан завантаження
- `selectAuthError` - помилка

## Використання в компонентах

```typescript
constructor(private store: Store) {
  this.loading$ = this.store.select(AuthSelectors.selectAuthLoading);
  this.error$ = this.store.select(AuthSelectors.selectAuthError);
}

onSubmit() {
  this.store.dispatch(AuthActions.login({
    email: this.formData.email,
    password: this.formData.password
  }));
}
```

## Guard для захищених роутів

```typescript
{
  path: 'admin',
  component: AdminComponent,
  canActivate: [authGuard]
}
```

## Тестування

1. Запустіть backend: `.\start-backend.ps1`
2. Запустіть frontend: `.\start-frontend.ps1`
3. Відкрийте: http://localhost:4200/login
4. Використовуйте NgRx DevTools для перегляду стану
