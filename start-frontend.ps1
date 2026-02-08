#!/usr/bin/env pwsh
# Start Angular Frontend

Write-Host "Starting CGG.WebClient..." -ForegroundColor Cyan

Set-Location -Path "$PSScriptRoot\CGG.WebClient"

# Check if node_modules exists
if (-not (Test-Path "node_modules")) {
    Write-Host "Installing npm packages..." -ForegroundColor Yellow
    npm install
}

npm start
