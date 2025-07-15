# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia arquivos de solução e projetos
COPY Skopia.sln ./
COPY Skopia.Api/Skopia.Api.csproj Skopia.Api/
COPY Skopia.Application/Skopia.Application.csproj Skopia.Application/
COPY Skopia.Infrastructure/Skopia.Infrastructure.csproj Skopia.Infrastructure/
COPY Skopia.Domain/Skopia.Domain.csproj Skopia.Domain/
COPY Skopia.DTOs/Skopia.DTOs.csproj Skopia.DTOs/
COPY Skopia.Tests/Skopia.Tests.csproj Skopia.Tests/

# Restaura pacotes SEM cache
RUN dotnet restore --no-cache

# Copia o restante dos arquivos e compila
COPY . .
WORKDIR /src/Skopia.Api
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Skopia.Api.dll"]

EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development