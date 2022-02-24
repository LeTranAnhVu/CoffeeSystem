# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["src/backends/OrderService/OrderService.csproj", "OrderService/"]
COPY ["src/backends/AuthForServicesExtension/AuthForServicesExtension.csproj", "AuthForServicesExtension/"]
COPY ["src/backends/RabbitMqServiceExtension/RabbitMqServiceExtension.csproj", "RabbitMqServiceExtension/"]

RUN dotnet restore "OrderService/OrderService.csproj"

# Copy everything else and build
COPY src/backends/OrderService/ OrderService/
COPY src/backends/AuthForServicesExtension/ AuthForServicesExtension/
COPY src/backends/RabbitMqServiceExtension/ RabbitMqServiceExtension/

WORKDIR /app/OrderService
RUN dotnet publish "OrderService.csproj" -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app/OrderService
COPY --from=build-env /app/OrderService/out .
ENTRYPOINT ["dotnet", "OrderService.dll"]