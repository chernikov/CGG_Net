#!/usr/bin/env pwsh
# Start both Backend and Frontend in parallel

Write-Host "Cleaning up existing services..." -ForegroundColor Yellow

# 1. Close terminal windows by title (set in start-backend.ps1 / start-frontend.ps1)
$windowTitles = @("CGG_Backend", "CGG_Frontend")
foreach ($title in $windowTitles) {
    $procs = Get-CimInstance Win32_Process -Filter "Name = 'pwsh.exe' OR Name = 'powershell.exe'" -ErrorAction SilentlyContinue |
             Where-Object { $_.CommandLine -match $title -or $_.CommandLine -match ($title -replace 'CGG_', 'start-') }
    foreach ($proc in $procs) {
        # Also kill child processes (dotnet, node)
        $children = Get-CimInstance Win32_Process -Filter "ParentProcessId = $($proc.ProcessId)" -ErrorAction SilentlyContinue
        foreach ($child in $children) {
            Stop-Process -Id $child.ProcessId -Force -ErrorAction SilentlyContinue
        }
        Stop-Process -Id $proc.ProcessId -Force -ErrorAction SilentlyContinue
        Write-Host "Closed window: $title (PID: $($proc.ProcessId))" -ForegroundColor DarkYellow
    }
}

# 2. Close any existing windows by script name (fallback)
$scripts = @("start-backend.ps1", "start-frontend.ps1")
foreach ($script in $scripts) {
    $procs = Get-CimInstance Win32_Process -Filter "Name = 'pwsh.exe' OR Name = 'powershell.exe'" -ErrorAction SilentlyContinue |
             Where-Object { $_.CommandLine -match [regex]::Escape($script) }
    foreach ($proc in $procs) {
        $children = Get-CimInstance Win32_Process -Filter "ParentProcessId = $($proc.ProcessId)" -ErrorAction SilentlyContinue
        foreach ($child in $children) {
            Stop-Process -Id $child.ProcessId -Force -ErrorAction SilentlyContinue
        }
        Stop-Process -Id $proc.ProcessId -Force -ErrorAction SilentlyContinue
        Write-Host "Closed process for $script (PID: $($proc.ProcessId))" -ForegroundColor DarkYellow
    }
}

# 3. Free up ports
$ports = @(7070, 5296, 4200)
foreach ($port in $ports) {
    $portPids = (Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue).OwningProcess | Select-Object -Unique
    foreach ($pid in $portPids) {
        Stop-Process -Id $pid -Force -ErrorAction SilentlyContinue
        Write-Host "Freed port $port (Killed PID: $pid)" -ForegroundColor DarkYellow
    }
}

Start-Sleep -Seconds 1

Write-Host ""
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
Write-Host "Backend API: https://localhost:7070" -ForegroundColor Yellow
Write-Host "Frontend: http://localhost:4200" -ForegroundColor Yellow
Write-Host ""
Write-Host "Press Ctrl+C in each terminal window to stop services" -ForegroundColor Gray
