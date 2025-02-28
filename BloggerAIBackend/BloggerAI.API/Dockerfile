#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BloggerAI.API/BloggerAI.API.csproj", "BloggerAI.API/"]
COPY ["BloggerAI.Core/BloggerAI.Core.csproj", "BloggerAI.Core/"]
COPY ["BloggerAI.Domain/BloggerAI.Domain.csproj", "BloggerAI.Domain/"]
COPY ["BloggerAI.Infrastructure/BloggerAI.Infrastructure.csproj", "BloggerAI.Infrastructure/"]
COPY ["BloggerAI.MSSQL/BloggerAI.MSSQL.csproj", "BloggerAI.MSSQL/"]
RUN dotnet restore "./BloggerAI.API/BloggerAI.API.csproj"
COPY . .
WORKDIR "/src/BloggerAI.API"
RUN dotnet build "./BloggerAI.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BloggerAI.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BloggerAI.API.dll"]