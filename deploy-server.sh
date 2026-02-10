#!/bin/bash

# ===================================
# CGG Deployment Script
# Server: cgg.eduzerone.com
# ===================================

set -e  # Exit on error

echo "==================================="
echo "CGG Deployment Starting..."
echo "==================================="
echo ""

# Colors for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

# Check if running as root or with sudo
if [ "$EUID" -ne 0 ]; then 
    echo -e "${RED}Please run as root or with sudo${NC}"
    exit 1
fi

# Navigate to deployment directory
DEPLOY_DIR="/home/deploy"
if [ ! -d "$DEPLOY_DIR" ]; then
    echo -e "${RED}Directory $DEPLOY_DIR does not exist!${NC}"
    exit 1
fi

cd $DEPLOY_DIR

# Check if .env.production exists
if [ ! -f ".env.production" ]; then
    echo -e "${RED}.env.production file not found in $DEPLOY_DIR${NC}"
    exit 1
fi

# Check if docker-compose.prod.yml exists
if [ ! -f "docker-compose.prod.yml" ]; then
    echo -e "${RED}docker-compose.prod.yml file not found in $DEPLOY_DIR${NC}"
    exit 1
fi

echo -e "${GREEN}✓${NC} Files found"
echo ""

# Pull latest images
echo -e "${YELLOW}Pulling latest Docker images...${NC}"
docker compose --env-file .env.production -f docker-compose.prod.yml pull
echo -e "${GREEN}✓${NC} Images pulled"
echo ""

# Stop existing containers
echo -e "${YELLOW}Stopping existing containers...${NC}"
docker compose --env-file .env.production -f docker-compose.prod.yml down
echo -e "${GREEN}✓${NC} Containers stopped"
echo ""

# Start containers
echo -e "${YELLOW}Starting containers...${NC}"
docker compose --env-file .env.production -f docker-compose.prod.yml up -d
echo -e "${GREEN}✓${NC} Containers started"
echo ""

# Wait for containers to be healthy
echo -e "${YELLOW}Waiting for containers to be ready...${NC}"
sleep 10

# Check container status
echo ""
echo -e "${YELLOW}Container Status:${NC}"
docker compose --env-file .env.production -f docker-compose.prod.yml ps
echo ""

# Show logs from API container
echo -e "${YELLOW}Recent API logs:${NC}"
docker compose --env-file .env.production -f docker-compose.prod.yml logs --tail=20 api
echo ""

# Check if nginx is installed
if ! command -v nginx &> /dev/null; then
    echo -e "${YELLOW}Nginx not installed. Installing...${NC}"
    apt update
    apt install -y nginx certbot python3-certbot-nginx
    echo -e "${GREEN}✓${NC} Nginx installed"
else
    echo -e "${GREEN}✓${NC} Nginx already installed"
fi

# Create nginx configuration if it doesn't exist
NGINX_CONF="/etc/nginx/sites-available/cgg.eduzerone.com"
if [ ! -f "$NGINX_CONF" ]; then
    echo -e "${YELLOW}Creating nginx configuration...${NC}"
    cat > $NGINX_CONF <<'EOF'
server {
    listen 80;
    server_name cgg.eduzerone.com;

    client_max_body_size 100M;

    location / {
        proxy_pass http://localhost:80;
        proxy_http_version 1.1;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_cache_bypass $http_upgrade;
    }
}
EOF
    
    # Enable site
    ln -sf $NGINX_CONF /etc/nginx/sites-enabled/cgg.eduzerone.com
    
    # Test nginx config
    nginx -t
    
    # Reload nginx
    systemctl reload nginx
    
    echo -e "${GREEN}✓${NC} Nginx configured"
else
    echo -e "${GREEN}✓${NC} Nginx configuration exists"
    nginx -t && systemctl reload nginx
fi

echo ""
echo -e "${GREEN}==================================="
echo "✓ Deployment Complete!"
echo "===================================${NC}"
echo ""
echo "Services:"
echo "  - Database: Running on port 1433 (internal)"
echo "  - API: Running on port 8080 (internal)"
echo "  - Web: Running on port 80"
echo "  - Site: http://cgg.eduzerone.com"
echo ""
echo "Management Commands:"
echo "  - View logs:        docker compose --env-file .env.production -f docker-compose.prod.yml logs -f"
echo "  - View API logs:    docker compose --env-file .env.production -f docker-compose.prod.yml logs -f api"
echo "  - View web logs:    docker compose --env-file .env.production -f docker-compose.prod.yml logs -f web"
echo "  - Restart all:      docker compose --env-file .env.production -f docker-compose.prod.yml restart"
echo "  - Stop all:         docker compose --env-file .env.production -f docker-compose.prod.yml down"
echo "  - Container status: docker compose --env-file .env.production -f docker-compose.prod.yml ps"
echo ""
echo "SSL Certificate:"
if [ ! -d "/etc/letsencrypt/live/cgg.eduzerone.com" ]; then
    echo -e "${YELLOW}  SSL certificate not configured yet.${NC}"
    echo "  Run: certbot --nginx -d cgg.eduzerone.com"
else
    echo -e "${GREEN}  ✓ SSL certificate installed${NC}"
fi
echo ""
echo -e "${YELLOW}Tip: View live logs with: docker compose --env-file .env.production -f docker-compose.prod.yml logs -f${NC}"
echo ""
