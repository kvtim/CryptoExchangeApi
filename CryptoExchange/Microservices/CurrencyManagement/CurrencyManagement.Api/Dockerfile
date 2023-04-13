#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Microservices/CurrencyManagement/CurrencyManagement.Api/CurrencyManagement.Api.csproj", "Microservices/CurrencyManagement/CurrencyManagement.Api/"]
COPY ["Microservices/CurrencyManagement/CurrencyManagement.Core/CurrencyManagement.Core.csproj", "Microservices/CurrencyManagement/CurrencyManagement.Core/"]
COPY ["Microservices/CurrencyManagement/CurrencyManagement.Data/CurrencyManagement.Data.csproj", "Microservices/CurrencyManagement/CurrencyManagement.Data/"]
COPY ["Microservices/CurrencyManagement/CurrencyManagement.Services/CurrencyManagement.Services.csproj", "Microservices/CurrencyManagement/CurrencyManagement.Services/"]
RUN dotnet restore "Microservices/CurrencyManagement/CurrencyManagement.Api/CurrencyManagement.Api.csproj"
COPY . .
WORKDIR "/src/Microservices/CurrencyManagement/CurrencyManagement.Api"
RUN dotnet build "CurrencyManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CurrencyManagement.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CurrencyManagement.Api.dll"]