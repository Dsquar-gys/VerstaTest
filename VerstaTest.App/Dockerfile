FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["VerstaTest.App/VerstaTest.App.csproj", "VerstaTest.App/"]
COPY ["VerstaTest.Infrastructure/VerstaTest.Infrastructure.csproj", "VerstaTest.Infrastructure/"]
COPY ["VerstaTest.Domain/VerstaTest.Domain.csproj", "VerstaTest.Domain/"]
RUN dotnet restore "VerstaTest.App/VerstaTest.App.csproj"
COPY . .
WORKDIR "/src/VerstaTest.App"
RUN dotnet build "./VerstaTest.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VerstaTest.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VerstaTest.App.dll"]
