# Career Guidance Guild - Startup Guide

## Quick Start

### Start Everything (Recommended)
```powershell
.\run-local-service.ps1
```
Запускає і Backend, і Frontend в окремих вікнах терміналу.

### Start Development Mode
```powershell
.\start-dev.ps1
```
Запускає обидва сервіси з автоматичним перезавантаженням при змінах в коді.

### Start Individual Services

#### Backend Only
```powershell
.\start-backend.ps1
```
Запускає .NET API на https://localhost:7000

#### Frontend Only
```powershell
.\start-frontend.ps1
```
Запускає Angular на http://localhost:4200

## Manual Startup

### Backend (.NET API)
```powershell
cd src/CGG.Api
dotnet run
```

або для розробки з watch mode:
```powershell
cd src/CGG.Api
dotnet watch run
```

### Frontend (Angular)
```powershell
cd CGG.WebClient
npm install  # тільки перший раз
npm start
```

## URLs
- **Frontend**: http://localhost:4200
- **Backend API**: https://localhost:7000
- **Swagger UI**: https://localhost:7000/swagger

## Requirements
- .NET 10.0 SDK
- Node.js 18+ і npm
- PowerShell 7+ (для скриптів)

## First Run
1. Встановіть залежності для Frontend:
   ```powershell
   cd CGG.WebClient
   npm install
   ```

2. Запустіть проєкт:
   ```powershell
   cd ..
   .\run-local-service.ps1
   ```

## Development
- Backend підтримує hot reload через `dotnet watch run`
- Frontend автоматично перезавантажується при змінах
- Використовуйте `start-dev.ps1` для найкращого досвіду розробки

## Troubleshooting

### Порти зайняті
Якщо порти 7000 або 4200 зайняті, змініть їх:
- Backend: `src/CGG.Api/Properties/launchSettings.json`
- Frontend: `CGG.WebClient/angular.json` (або `--port 4300`)

### npm install помилки
```powershell
cd CGG.WebClient
rm -r node_modules
npm cache clean --force
npm install
```

### .NET build помилки
```powershell
cd src/CGG.Api
dotnet clean
dotnet restore
dotnet build
```
