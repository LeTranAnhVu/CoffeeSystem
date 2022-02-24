# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["src/backends/ProductService/ProductService.csproj", "ProductService/"]
COPY ["src/backends/AuthForServicesExtension/AuthForServicesExtension.csproj", "AuthForServicesExtension/"]
RUN dotnet restore "ProductService/ProductService.csproj"

# Copy everything else and build
COPY src/backends/ProductService/ ProductService/
COPY src/backends/AuthForServicesExtension/ AuthForServicesExtension/
WORKDIR /app/ProductService
RUN dotnet publish "ProductService.csproj" -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app/ProductService
COPY --from=build-env /app/ProductService/out .
ENTRYPOINT ["dotnet", "ProductService.dll"]