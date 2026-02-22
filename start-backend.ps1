#!/usr/bin/env pwsh
# Start .NET API Backend

$Host.UI.RawUI.WindowTitle = "CGG_Backend"

Write-Host "Starting CGG.Api..." -ForegroundColor Cyan

Set-Location -Path "$PSScriptRoot\src\CGG.Api"

dotnet run --launch-profile "https"
