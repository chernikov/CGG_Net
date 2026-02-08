#!/usr/bin/env pwsh
# Start .NET API Backend

Write-Host "Starting CGG.Api..." -ForegroundColor Cyan

Set-Location -Path "$PSScriptRoot\src\CGG.Api"

dotnet run --launch-profile "https"
