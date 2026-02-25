FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/Directory.Build.props ./src/Directory.Build.props
COPY src/Directory.Packages.props ./src/Directory.Packages.props
COPY src/CGG.Api/CGG.Api.csproj ./src/CGG.Api/
COPY src/CGG.Application/CGG.Application.csproj ./src/CGG.Application/
COPY src/CGG.Infrastructure/CGG.Infrastructure.csproj ./src/CGG.Infrastructure/
COPY src/CGG.Core/CGG.Core.csproj ./src/CGG.Core/

RUN dotnet restore ./src/CGG.Api/CGG.Api.csproj

COPY src/ ./src/
RUN dotnet publish ./src/CGG.Api/CGG.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CGG.Api.dll"]
