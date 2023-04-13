#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Microservices/FinanceManagement/FinanceManagement.Api/FinanceManagement.Api.csproj", "Microservices/FinanceManagement/FinanceManagement.Api/"]
COPY ["Common/EventBus/EventBus.Messages/EventBus.Messages.csproj", "Common/EventBus/EventBus.Messages/"]
COPY ["Microservices/FinanceManagement/FinanceManagement.Core/FinanceManagement.Core.csproj", "Microservices/FinanceManagement/FinanceManagement.Core/"]
COPY ["Microservices/FinanceManagement/FinanceManagement.Data/FinanceManagement.Data.csproj", "Microservices/FinanceManagement/FinanceManagement.Data/"]
RUN dotnet restore "Microservices/FinanceManagement/FinanceManagement.Api/FinanceManagement.Api.csproj"
COPY . .
WORKDIR "/src/Microservices/FinanceManagement/FinanceManagement.Api"
RUN dotnet build "FinanceManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinanceManagement.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinanceManagement.Api.dll"]