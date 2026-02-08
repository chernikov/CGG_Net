#!/usr/bin/env pwsh
# Development mode startup with watch

Write-Host "Starting in Development Mode..." -ForegroundColor Green
Write-Host "=================================" -ForegroundColor Green
Write-Host ""

# Start Backend with watch in new terminal
Write-Host "Starting Backend API (watch mode)..." -ForegroundColor Cyan
Start-Process pwsh -ArgumentList "-NoExit", "-Command", "cd '$PSScriptRoot\src\CGG.Api'; dotnet watch run"

# Wait for backend
Start-Sleep -Seconds 3

# Start Frontend in new terminal
Write-Host "Starting Frontend (ng serve)..." -ForegroundColor Cyan
Start-Process pwsh -ArgumentList "-NoExit", "-File", "$PSScriptRoot\start-frontend.ps1"

Write-Host ""
Write-Host "=================================" -ForegroundColor Green
Write-Host "Development servers are running" -ForegroundColor Green
Write-Host "Backend: https://localhost:7000 (auto-reload)" -ForegroundColor Yellow
Write-Host "Frontend: http://localhost:4200 (auto-reload)" -ForegroundColor Yellow
Write-Host ""
Write-Host "Changes will be detected automatically" -ForegroundColor Cyan
