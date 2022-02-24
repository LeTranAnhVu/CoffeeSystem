# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["src/backends/SignalRService/SignalRService.csproj", "SignalRService/"]
COPY ["src/backends/AuthForServicesExtension/AuthForServicesExtension.csproj", "AuthForServicesExtension/"]
COPY ["src/backends/RabbitMqServiceExtension/RabbitMqServiceExtension.csproj", "RabbitMqServiceExtension/"]
RUN dotnet restore "SignalRService/SignalRService.csproj"

# Copy everything else and build
COPY src/backends/SignalRService/ SignalRService/
COPY src/backends/AuthForServicesExtension/ AuthForServicesExtension/
COPY src/backends/RabbitMqServiceExtension/ RabbitMqServiceExtension/
WORKDIR /app/SignalRService
RUN dotnet publish "SignalRService.csproj" -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app/SignalRService
COPY --from=build-env /app/SignalRService/out .
ENTRYPOINT ["dotnet", "SignalRService.dll"]