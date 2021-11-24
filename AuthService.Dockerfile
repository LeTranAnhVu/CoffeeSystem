# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["AuthService/AuthService.csproj", "AuthService/"]
RUN dotnet restore "AuthService/AuthService.csproj"

# Copy everything else and build
COPY AuthService/ AuthService/
WORKDIR /app/AuthService
RUN dotnet publish "AuthService.csproj" -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app/AuthService
COPY --from=build-env /app/AuthService/out .
ENTRYPOINT ["dotnet", "AuthService.dll"]