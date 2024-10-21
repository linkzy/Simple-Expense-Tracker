# Use the ASP.NET runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the .NET SDK image for building the project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy all the project files that are referenced by the Server project
COPY ["SimpleExpenseTracker/SimpleExpenseTracker/Server/SimpleExpenseTracker.Server.csproj", "SimpleExpenseTracker/Server/"]
COPY ["SimpleExpenseTracker/SimpleExpenseTracker.Client/SimpleExpenseTracker.Client.csproj", "SimpleExpenseTracker/Client/"]
COPY ["SimpleExpenseTracker/SimpleExpenseTracker.Domain/SimpleExpenseTracker.Domain.csproj", "SimpleExpenseTracker.Domain/"]
COPY ["SimpleExpenseTracker/SimpleExpenseTracker.Infra/SimpleExpenseTracker.Infra.csproj", "SimpleExpenseTracker.Infra/"]
COPY ["SimpleExpenseTracker/SimpleExpenseTracker.Shared/SimpleExpenseTracker.Shared.csproj", "SimpleExpenseTracker/Shared/"]

# Restore the dependencies
RUN dotnet restore "./SimpleExpenseTracker/Server/./SimpleExpenseTracker.Server.csproj"

# Copy the rest of the application files
COPY . .

# Set the working directory to the Server project and build it
WORKDIR "/src/SimpleExpenseTracker/Server"
RUN dotnet build "SimpleExpenseTracker.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the build output
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SimpleExpenseTracker.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Use the final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleExpenseTracker.Server.dll"]
