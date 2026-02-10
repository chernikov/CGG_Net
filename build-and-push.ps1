# Build and Push Docker Images to Docker Hub
# Usage: .\build-and-push.ps1 [-Tag "latest"]

param(
    [string]$Tag = "latest"
)

# Check if Docker is running
if (-not (docker ps 2>$null)) {
    Write-Error "Docker is not running. Please start Docker and try again."
    exit 1
}

# Read Docker Hub username from .env.production
$DockerHubUsername = "chernikov"  # Default
if (Test-Path ".env.production") {
    $envContent = Get-Content ".env.production"
    foreach ($line in $envContent) {
        if ($line -match "^DOCKERHUB_USERNAME=(.+)$") {
            $DockerHubUsername = $matches[1].Trim()
            break
        }
    }
}

Write-Host ""
Write-Host "=== Building and Pushing Images to Docker Hub ===" -ForegroundColor Cyan
Write-Host "Username: $DockerHubUsername" -ForegroundColor Yellow
Write-Host "Tag: $Tag" -ForegroundColor Yellow
Write-Host ""

# Login to Docker Hub
Write-Host "Logging in to Docker Hub..." -ForegroundColor Cyan
docker login

if ($LASTEXITCODE -ne 0) {
    Write-Error "Docker login failed. Please check your credentials."
    exit 1
}

Write-Host ""
Write-Host "=== Building CGG API ===" -ForegroundColor Green
docker build -f src/CGG.Api/Dockerfile -t ${DockerHubUsername}/cgg-api:${Tag} .

if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to build API image"
    exit 1
}

Write-Host ""
Write-Host "Pushing API image to Docker Hub..." -ForegroundColor Green
docker push ${DockerHubUsername}/cgg-api:${Tag}

if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to push API image"
    exit 1
}

Write-Host ""
Write-Host "=== Building CGG Web ===" -ForegroundColor Green
docker build -f CGG.WebClient/Dockerfile -t ${DockerHubUsername}/cgg-web:${Tag} .

if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to build Web image"
    exit 1
}

Write-Host ""
Write-Host "Pushing Web image to Docker Hub..." -ForegroundColor Green
docker push ${DockerHubUsername}/cgg-web:${Tag}

if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to push Web image"
    exit 1
}

Write-Host ""
Write-Host "=== ✅ All images built and pushed successfully! ===" -ForegroundColor Green
Write-Host ""
Write-Host "Images:" -ForegroundColor Cyan
Write-Host "  - ${DockerHubUsername}/cgg-api:${Tag}" -ForegroundColor Yellow
Write-Host "  - ${DockerHubUsername}/cgg-web:${Tag}" -ForegroundColor Yellow
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host "1. SSH to server: ssh root@46.101.247.177" -ForegroundColor White
Write-Host "2. Run deployment: cd /home/deploy && ./deploy-server.sh" -ForegroundColor White
Write-Host ""
