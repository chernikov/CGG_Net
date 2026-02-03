# Career Guidance Guild

A magical career guidance platform that helps users discover their professional path through an engaging, interactive experience.

## Features

- 🌟 Magical Quest flow with animated UI elements
- 🌍 Multilingual support (Ukrainian, English, Hindi)
- 📱 Fully responsive design
- 🔮 Interactive survey with progress tracking
- 🤖 ChatGPT integration for personalized recommendations
- 🪄 Supports OpenAI GPT-5 Nano for faster responses
- 🔥 Firebase integration for session management
- 🎨 Beautiful, modern UI with magical theme
- 💳 Payment system with Monobank integration
- 📧 Email notifications via Resend
- 👤 User credit system and balance management

## Tech Stack

- Next.js 15.3.3
- TypeScript 5.x
- TailwindCSS 3.x
- Framer Motion 11.x
- Firebase (Auth + Firestore) 10.x
- i18next 23.x
- OpenAI API v1
- Resend (Email service)
- Monobank API (Payments)

## Getting Started


1. Clone the repository
2. Install dependencies:
   ```bash
   cd client
   pnpm install
   ```
3. Copy `.env.example` to `.env.local` and configure:
   ```bash
   cp .env.example .env.local
   ```
   Required variables:
   - Firebase configuration
   - OpenAI API key
   - Resend API key (see [RESEND_SETUP.md](./RESEND_SETUP.md))
   - Monobank token (for payments)
   - JWT secret

4. Run the development server:
   ```bash
   cd client
   pnpm dev
   ```

## Docker Deployment

### Quick Start with Docker

**Build and run locally:**
```powershell
# Build image
.\build-docker.ps1 -Environment dev

# Run container
.\run-docker.ps1 -Environment dev -Port 8080
```

**Or using Docker Compose:**
```powershell
# With helper script
.\docker-compose-helper.ps1 -Action up -Environment dev -Build

# Or directly
docker-compose --env-file .env.local up --build -d
```

📖 **Detailed instructions:** See [DOCKER.md](./DOCKER.md)

## Email Setup (Resend)

The project uses **Resend** for email notifications:
- Payment confirmations
- Low balance alerts
- Admin credit notifications

📧 **Setup guide:** See [RESEND_SETUP.md](./RESEND_SETUP.md)

## Docker & Cloud Run (Legacy)

- Для продакшн-збірки та деплою всі змінні з `.env.local` автоматично підтягуються у Docker build та Cloud Run через PowerShell-скрипти.
- Для збірки Docker-образу використовуйте:
  ```powershell
  ./build.ps1
  ```
  Скрипт зчитує всі змінні з `.env.local` і передає їх у Docker як build-arg.
- Для деплою на Google Cloud Run використовуйте:
  ```powershell
  ./deploy-cloudrun.ps1
  ```
  Скрипт автоматично підхоплює всі змінні з `.env.local` і передає їх як build-arg (для build) та як env variables (для runtime) у Cloud Run.

> **Note:** Якщо ви змінюєте змінні у `.env.local`, потрібно перебудувати Docker-образ для оновлення build-time змінних.

## API Endpoints

### GET /api/user-info
Returns current authenticated user basic info.

Headers:
- Authorization: Bearer <Firebase ID Token>

Response JSON:
```
{
  "success": true,
  "user": {
    "uid": "string",
    "email": "user@example.com",
    "displayName": "Name",
    "credits": 25
  }
}
```
Error (401):
```
{ "success": false, "error": "..." }
```

### Caching User Info
The app now caches authenticated user basics in `localStorage` under key `user-info`:
```
{
  "data": { "uid": "...", "email": "...", "displayName": "...", "credits": 25 },
  "cachedAt": 1730000000000,
  "ttlMs": 60000
}
```
Use `credits` from this object to get the user's current balance. A hook `useUserInfo` is available at `src/hooks/user/useUserInfo.ts`.

## Project Structure

```
src/
├── app/                    # Next.js app directory
│   ├── components/        # Reusable components
│   ├── magical-quest/     # Magical Quest flow
│   └── ...
├── config/                # Configuration files
├── hooks/                 # Custom React hooks
├── lib/                   # Utility functions
├── locales/              # Translation files
└── types/                # TypeScript types
```

## Contributing

1. Fork the repository
2. Create your feature branch
3. Commit your changes
4. Push to the branch
5. Create a new Pull Request

## License

MIT
