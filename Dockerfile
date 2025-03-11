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

# Copy the entire project structure
COPY host/ host/
COPY src/ src/

# Restore dependencies
WORKDIR /src/host/DayPay.Host
RUN dotnet restore "DayPay.Host.csproj"

# Build the project
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
