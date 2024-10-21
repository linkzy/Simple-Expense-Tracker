#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SimpleExpenseTracker/SimpleExpenseTracker/Server/SimpleExpenseTracker.Server.csproj", "SimpleExpenseTracker/Server/"]
COPY ["SimpleExpenseTracker/SimpleExpenseTracker.Domain/SimpleExpenseTracker.Domain.csproj", "SimpleExpenseTracker.Domain/"]
COPY ["SimpleExpenseTracker/SimpleExpenseTracker.Infra/SimpleExpenseTracker.Infra.csproj", "SimpleExpenseTracker.Infra/"]
COPY ["SimpleExpenseTracker/SimpleExpenseTracker/Client/SimpleExpenseTracker.Client.csproj", "SimpleExpenseTracker/Client/"]
COPY ["SimpleExpenseTracker/SimpleExpenseTracker/Shared/SimpleExpenseTracker.Shared.csproj", "SimpleExpenseTracker/Shared/"]
RUN dotnet restore "./SimpleExpenseTracker/Server/./SimpleExpenseTracker.Server.csproj"
COPY . .
WORKDIR "/src/SimpleExpenseTracker/Server"
RUN dotnet build "./SimpleExpenseTracker.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SimpleExpenseTracker.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleExpenseTracker.Server.dll"]
