# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["ApiGateWay/ApiGateWay.csproj", "ApiGateWay/"]
COPY ["AuthForServicesExtension/AuthForServicesExtension.csproj", "AuthForServicesExtension/"]
RUN dotnet restore "ApiGateWay/ApiGateWay.csproj"

# Copy everything else and build
COPY ApiGateWay/ ApiGateWay/
COPY AuthForServicesExtension/ AuthForServicesExtension/
WORKDIR /app/ApiGateWay
RUN dotnet publish "ApiGateWay.csproj" -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app/ApiGateWay
COPY --from=build-env /app/ApiGateWay/out .
ENTRYPOINT ["dotnet", "ApiGateWay.dll"]