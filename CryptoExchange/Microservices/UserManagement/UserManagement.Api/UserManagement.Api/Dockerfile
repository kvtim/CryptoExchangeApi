#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Microservices/UserManagement/UserManagement.Api/UserManagement.Api/UserManagement.Api.csproj", "Microservices/UserManagement/UserManagement.Api/UserManagement.Api/"]
COPY ["Common/EventBus/EventBus.Messages/EventBus.Messages.csproj", "Common/EventBus/EventBus.Messages/"]
COPY ["Microservices/UserManagement/UserManagement.Core/UserManagement.Core.csproj", "Microservices/UserManagement/UserManagement.Core/"]
COPY ["Microservices/UserManagement/UserManagement.Data/UserManagement.Data.csproj", "Microservices/UserManagement/UserManagement.Data/"]
COPY ["Microservices/UserManagement/UserManagement.Services/UserManagement.Services.csproj", "Microservices/UserManagement/UserManagement.Services/"]
RUN dotnet restore "Microservices/UserManagement/UserManagement.Api/UserManagement.Api/UserManagement.Api.csproj"
COPY . .
WORKDIR "/src/Microservices/UserManagement/UserManagement.Api/UserManagement.Api"
RUN dotnet build "UserManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserManagement.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserManagement.Api.dll"]