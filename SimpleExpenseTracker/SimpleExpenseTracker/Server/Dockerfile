#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SimpleExpenseTracker/Server/SimpleExpenseTracker.Server.csproj", "SimpleExpenseTracker/Server/"]
COPY ["SimpleExpenseTracker.Domain/SimpleExpenseTracker.Domain.csproj", "SimpleExpenseTracker.Domain/"]
COPY ["SimpleExpenseTracker.Infra/SimpleExpenseTracker.Infra.csproj", "SimpleExpenseTracker.Infra/"]
COPY ["SimpleExpenseTracker/Client/SimpleExpenseTracker.Client.csproj", "SimpleExpenseTracker/Client/"]
COPY ["SimpleExpenseTracker/Shared/SimpleExpenseTracker.Shared.csproj", "SimpleExpenseTracker/Shared/"]
RUN dotnet restore "SimpleExpenseTracker/Server/SimpleExpenseTracker.Server.csproj"
COPY . .
WORKDIR "/src/SimpleExpenseTracker/Server"
RUN dotnet build "SimpleExpenseTracker.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleExpenseTracker.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleExpenseTracker.Server.dll"]