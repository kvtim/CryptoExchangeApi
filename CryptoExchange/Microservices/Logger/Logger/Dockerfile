#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5005

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Microservices/Logger/Logger/Logger.csproj", "Microservices/Logger/Logger/"]
COPY ["Common/EventBus/EventBus.Messages/EventBus.Messages.csproj", "Common/EventBus/EventBus.Messages/"]
RUN dotnet restore "Microservices/Logger/Logger/Logger.csproj"
COPY . .
WORKDIR "/src/Microservices/Logger/Logger"
RUN dotnet build "Logger.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Logger.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Logger.dll"]