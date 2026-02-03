# 🔥 Firebase Database API Documentation

## Розташування файлу
**`/client/src/app/api/db/route.ts`**

## Опис
Універсальний API endpoint для роботи з Firebase Firestore Database через серверну частину Next.js.

## Базова URL
```
POST http://localhost:3000/api/db
```

## Структура запиту

### Загальний формат
```typescript
{
  "action": string,           // Тип операції
  "collection": string,       // Назва колекції
  "documentId"?: string,      // ID документа (опціонально)
  "data"?: any,              // Дані (для create/update)
  "options"?: {              // Додаткові опції
    "limit"?: number,
    "filters"?: Filter[],
    "orderBy"?: OrderBy[]
  }
}
```

### Типи фільтрів
```typescript
type Filter = {
  field: string,
  operator: '==' | '!=' | '>' | '>=' | '<' | '<=' | 'in' | 'not-in' | 'array-contains',
  value: any
}

type OrderBy = {
  field: string,
  direction: 'asc' | 'desc'
}
```

## 📋 Доступні операції

### 1. **CREATE** - Створити документ

#### Запит:
```json
{
  "action": "create",
  "collection": "users",
  "data": {
    "name": "John Doe",
    "email": "john@example.com",
    "age": 30
  },
  "documentId": "optional-custom-id"
}
```

#### Відповідь:
```json
{
  "success": true,
  "docId": "auto-generated-id-or-custom-id"
}
```

### 2. **READ** - Читати документ

#### Запит:
```json
{
  "action": "read",
  "collection": "users",
  "documentId": "user123"
}
```

#### Відповідь:
```json
{
  "success": true,
  "data": {
    "id": "user123",
    "name": "John Doe",
    "email": "john@example.com",
    "age": 30,
    "createdAt": "2025-01-26T10:30:00.000Z"
  }
}
```

### 3. **READ** - Читати колекцію

#### Запит (базовий):
```json
{
  "action": "read",
  "collection": "users"
}
```

#### Запит (з фільтрами та сортуванням):
```json
{
  "action": "read",
  "collection": "users",
  "options": {
    "limit": 10,
    "filters": [
      {
        "field": "age",
        "operator": ">=",
        "value": 18
      },
      {
        "field": "status",
        "operator": "==",
        "value": "active"
      }
    ],
    "orderBy": [
      {
        "field": "createdAt",
        "direction": "desc"
      }
    ]
  }
}
```

#### Відповідь:
```json
{
  "success": true,
  "data": [
    {
      "id": "user1",
      "name": "John Doe",
      "age": 30,
      "status": "active"
    },
    {
      "id": "user2", 
      "name": "Jane Smith",
      "age": 25,
      "status": "active"
    }
  ],
  "count": 2
}
```

### 4. **UPDATE** - Оновити документ

#### Запит:
```json
{
  "action": "update",
  "collection": "users",
  "documentId": "user123",
  "data": {
    "age": 31,
    "lastLogin": "2025-01-26T15:30:00.000Z"
  }
}
```

#### Відповідь:
```json
{
  "success": true
}
```

### 5. **DELETE** - Видалити документ

#### Запит:
```json
{
  "action": "delete",
  "collection": "users",
  "documentId": "user123"
}
```

#### Відповідь:
```json
{
  "success": true
}
```

### 6. **DELETE** - Видалити колекцію

#### Запит (без documentId - видаляє всю колекцію):
```json
{
  "action": "delete",
  "collection": "temp_data"
}
```

#### Відповідь:
```json
{
  "success": true,
  "deletedCount": 15
}
```

### 7. **TEST** - Тест з'єднання

#### Запит:
```json
{
  "action": "test"
}
```

#### Відповідь:
```json
{
  "success": true,
  "projectId": "cgg-v1"
}
```

## 🚨 Обробка помилок

### Формат помилки:
```json
{
  "success": false,
  "error": "Error message"
}
```

### Типові помилки:

#### 400 - Bad Request
```json
{
  "success": false,
  "error": "Action and collection are required"
}
```

```json
{
  "success": false,
  "error": "Data is required for create action"
}
```

```json
{
  "success": false,
  "error": "DocumentId and data are required for update action"
}
```

```json
{
  "success": false,
  "error": "Invalid action. Use: create, read, update, delete, test"
}
```

#### 500 - Server Error
```json
{
  "success": false,
  "error": "Firebase connection failed"
}
```

## 💡 Приклади використання

### В React компоненті:

```typescript
// Створити користувача
const createUser = async (userData: any) => {
  const response = await fetch('/api/db', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      action: 'create',
      collection: 'users',
      data: userData
    }),
  });
  
  const result = await response.json();
  return result;
};

// Отримати активних користувачів
const getActiveUsers = async () => {
  const response = await fetch('/api/db', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',  
    },
    body: JSON.stringify({
      action: 'read',
      collection: 'users',
      options: {
        filters: [
          { field: 'status', operator: '==', value: 'active' }
        ],
        orderBy: [
          { field: 'name', direction: 'asc' }
        ],
        limit: 50
      }
    }),
  });
  
  const result = await response.json();
  return result.data;
};

// Оновити користувача
const updateUser = async (userId: string, updateData: any) => {
  const response = await fetch('/api/db', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      action: 'update',
      collection: 'users',
      documentId: userId,
      data: updateData
    }),
  });
  
  return await response.json();
};

// Видалити користувача
const deleteUser = async (userId: string) => {
  const response = await fetch('/api/db', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      action: 'delete',
      collection: 'users',
      documentId: userId
    }),
  });
  
  return await response.json();
};
```

### В Server Component:

```typescript
import { firebaseDB } from '@/lib/firebaseDB';

// Прямий виклик сервісу (без API)
export default async function UsersPage() {
  const users = await firebaseDB.readCollection('users', {
    filters: [{ field: 'status', operator: '==', value: 'active' }]
  });
  
  return (
    <div>
      {users.data?.map(user => (
        <div key={user.id}>{user.name}</div>
      ))}
    </div>
  );
}
```

### В API Route:

```typescript
// app/api/users/route.ts
import { firebaseDB } from '@/lib/firebaseDB';

export async function GET() {
  const users = await firebaseDB.readCollection('users');
  return Response.json(users);
}

export async function POST(request: Request) {
  const userData = await request.json();
  const result = await firebaseDB.createDocument('users', userData);
  return Response.json(result);
}
```

## 🔧 Налаштування

### Environment Variables:
```env
NEXT_PUBLIC_FIREBASE_API_KEY=your-api-key
NEXT_PUBLIC_FIREBASE_AUTH_DOMAIN=your-project.firebaseapp.com
NEXT_PUBLIC_FIREBASE_PROJECT_ID=your-project-id
NEXT_PUBLIC_FIREBASE_STORAGE_BUCKET=your-project.appspot.com
NEXT_PUBLIC_FIREBASE_MESSAGING_SENDER_ID=123456789
NEXT_PUBLIC_FIREBASE_APP_ID=1:123456789:web:abcdef123456
```

## 📚 Додаткові можливості

### Пагінація:
Для великих колекцій використовуйте `limit` та комбінуйте з `orderBy`:

```json
{
  "action": "read",
  "collection": "posts",
  "options": {
    "limit": 20,
    "orderBy": [{"field": "createdAt", "direction": "desc"}]
  }
}
```

### Складні запити:
Можна комбінувати кілька фільтрів:

```json
{
  "action": "read", 
  "collection": "products",
  "options": {
    "filters": [
      {"field": "category", "operator": "==", "value": "electronics"},
      {"field": "price", "operator": "<=", "value": 1000},
      {"field": "inStock", "operator": "==", "value": true}
    ]
  }
}
```

### Автоматичні поля:
Сервіс автоматично додає до всіх документів:
- `createdAt` - дата створення (ISO string)
- `updatedAt` - дата останнього оновлення (ISO string)

## 🎯 Архітектура

### Файли:
- `/client/src/app/api/db/route.ts` - API endpoint
- `/client/src/lib/firebaseDB.ts` - основний сервіс
- `/client/src/config/firebaseClient.ts` - конфігурація Firebase

### Потік даних:
```
React Component → /api/db → firebaseDB Service → Firebase SDK → Firestore
Server Component → firebaseDB Service → Firebase SDK → Firestore
```

## 🔒 Безпека

- Всі Firebase credentials зберігаються в environment variables
- Клієнт не має прямого доступу до Firebase Admin SDK
- API може бути розширений middleware для аутентифікації та авторизації

## 📝 TODO / Можливі покращення

- [ ] Додати аутентифікацію до API endpoints
- [ ] Реалізувати rate limiting
- [ ] Додати валідацію схем даних
- [ ] Реалізувати real-time subscriptions
- [ ] Додати batch operations
- [ ] Покращити error handling з детальними кодами помилок
