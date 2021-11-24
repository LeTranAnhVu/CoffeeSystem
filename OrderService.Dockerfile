# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["OrderService/OrderService.csproj", "OrderService/"]
COPY ["AuthForServicesExtension/AuthForServicesExtension.csproj", "AuthForServicesExtension/"]
RUN dotnet restore "OrderService/OrderService.csproj"

# Copy everything else and build
COPY OrderService/ OrderService/
COPY AuthForServicesExtension/ AuthForServicesExtension/
WORKDIR /app/OrderService
RUN dotnet publish "OrderService.csproj" -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app/OrderService
COPY --from=build-env /app/OrderService/out .
ENTRYPOINT ["dotnet", "OrderService.dll"]