FROM node:20-alpine AS build
WORKDIR /app

COPY CGG.WebClient/package*.json ./
RUN npm ci

COPY CGG.WebClient/ ./
RUN npm run build -- --output-path=dist-app

FROM nginx:alpine
COPY CGG.WebClient/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist-app /usr/share/nginx/html
