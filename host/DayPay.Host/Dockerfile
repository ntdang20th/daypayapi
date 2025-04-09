FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release

WORKDIR /src

COPY ["host/DayPay.Host/DayPay.Host.csproj", "host/DayPay.Host/"]
COPY ["src/DayPay.Application/DayPay.Application.csproj", "src/DayPay.Application/"]
COPY ["src/DayPay.Application.Contracts/DayPay.Application.Contracts.csproj", "src/DayPay.Application.Contracts/"]
COPY ["src/DayPay.Application.Redis/DayPay.Application.Redis.csproj", "src/DayPay.Application.Redis/"]
COPY ["src/DayPay.Application.ElasticSearch/DayPay.Application.ElasticSearch.csproj", "src/DayPay.Application.ElasticSearch/"]
COPY ["src/DayPay.Domain/DayPay.Domain.csproj", "src/DayPay.Domain/"]
COPY ["src/DayPay.Domain.Shared/DayPay.Domain.Shared.csproj", "src/DayPay.Domain.Shared/"]
COPY ["src/DayPay.EntityFrameworkCore/DayPay.EntityFrameworkCore.csproj", "src/DayPay.EntityFrameworkCore/"]
COPY ["src/DayPay.HttpApi/DayPay.HttpApi.csproj", "src/DayPay.HttpApi/"]


RUN dotnet restore "host/DayPay.Host/DayPay.Host.csproj"
COPY . .

WORKDIR "/src/host/DayPay.Host"

RUN dotnet build "DayPay.Host.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release

RUN dotnet publish "DayPay.Host.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DayPay.Host.dll"]
