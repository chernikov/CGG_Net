# Career Guidance Guild - Angular Frontend

## Розробка

Запуск dev сервера:
```bash
ng serve
```

Відкрийте `http://localhost:4200`

## Збірка

Production збірка:
```bash
ng build --configuration production
```

## Структура проєкту

```
src/
├── app/
│   ├── core/              # Основні сервіси, guards, interceptors
│   ├── shared/            # Загальні компоненти
│   ├── features/          # Feature модулі
│   │   ├── auth/         # Аутентифікація
│   │   ├── surveys/      # Опитування
│   │   ├── profile/      # Профіль користувача
│   │   └── admin/        # Адмін панель
│   └── pages/            # Сторінки
├── environments/          # Конфігурація середовищ
└── styles.scss           # Глобальні стилі
```

## API Backend

Backend API має бути запущений на `https://localhost:7001`

Переконайтесь що:
1. SQL Server запущений
2. База даних створена
3. API проєкт запущений

## Налаштування

Відредагуйте `src/environments/environment.ts` для налаштування API URL та інших параметрів.
