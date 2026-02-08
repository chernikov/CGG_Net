#!/usr/bin/env pwsh
# Start both Backend and Frontend in parallel

Write-Host "Starting Career Guidance Guild..." -ForegroundColor Green
Write-Host "=================================" -ForegroundColor Green
Write-Host ""

# Start Backend in new terminal
Write-Host "Starting Backend API..." -ForegroundColor Cyan
Start-Process pwsh -ArgumentList "-NoExit", "-File", "$PSScriptRoot\start-backend.ps1"

# Wait a bit for backend to start
Start-Sleep -Seconds 3

# Start Frontend in new terminal
Write-Host "Starting Frontend..." -ForegroundColor Cyan
Start-Process pwsh -ArgumentList "-NoExit", "-File", "$PSScriptRoot\start-frontend.ps1"

Write-Host ""
Write-Host "=================================" -ForegroundColor Green
Write-Host "Both services are starting..." -ForegroundColor Green
Write-Host "Backend API: https://localhost:7000" -ForegroundColor Yellow
Write-Host "Frontend: http://localhost:4200" -ForegroundColor Yellow
Write-Host ""
Write-Host "Press Ctrl+C in each terminal window to stop services" -ForegroundColor Gray
