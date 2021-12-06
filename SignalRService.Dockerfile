# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["SignalRService/SignalRService.csproj", "SignalRService/"]
COPY ["AuthForServicesExtension/AuthForServicesExtension.csproj", "AuthForServicesExtension/"]
COPY ["RabbitMqServiceExtension/RabbitMqServiceExtension.csproj", "RabbitMqServiceExtension/"]
RUN dotnet restore "SignalRService/SignalRService.csproj"

# Copy everything else and build
COPY SignalRService/ SignalRService/
COPY AuthForServicesExtension/ AuthForServicesExtension/
COPY RabbitMqServiceExtension/ RabbitMqServiceExtension/
WORKDIR /app/SignalRService
RUN dotnet publish "SignalRService.csproj" -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app/SignalRService
COPY --from=build-env /app/SignalRService/out .
ENTRYPOINT ["dotnet", "SignalRService.dll"]