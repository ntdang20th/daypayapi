# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

# Set the working directory for the build
WORKDIR /src

# Copy the host project file into the correct location
COPY ["host/DayPay.Host/DayPay.Host.csproj", "DayPay/host/DayPay.Host/"]
COPY ["host/DayPay.Host/appsettings.json", "DayPay/host/DayPay.Host/"]

# Copy dependency project files based on the specified structure
COPY ["src/DayPay.Application/DayPay.Application.csproj", "DayPay/src/DayPay.Application/"]
COPY ["src/DayPay.HttpApi/DayPay.HttpApi.csproj", "DayPay/src/DayPay.HttpApi/"]
COPY ["src/DayPay.EntityFrameworkCore/DayPay.EntityFrameworkCore.csproj", "DayPay/src/DayPay.EntityFrameworkCore/"]
COPY ["src/DayPay.Application.Contracts/DayPay.Application.Contracts.csproj", "DayPay/src/DayPay.Application.Contracts/"]
COPY ["src/DayPay.Domain/DayPay.Domain.csproj", "DayPay/src/DayPay.Domain/"]
COPY ["src/DayPay.Domain.Shared/DayPay.Domain.Shared.csproj", "DayPay/src/DayPay.Domain.Shared/"]
COPY ["src/DayPay.HttpApi.Client/DayPay.HttpApi.Client.csproj", "DayPay/src/DayPay.HttpApi.Client/"]

# Run restore using the main project file in the host directory
WORKDIR /src/DayPay/host/DayPay.Host
RUN dotnet restore "DayPay.Host.csproj"

# Copy the remaining source code into the Docker image
COPY . .

# Set the working directory to the host project and build it
RUN dotnet build "DayPay.Host.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DayPay.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage for runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DayPay.Host.dll"]
