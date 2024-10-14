FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5050

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Guids.Api.Client/Guids.Api.Client.csproj", "Guids.Api.Client/"]
RUN dotnet restore "Guids.Api.Client/Guids.Api.Client.csproj"
COPY . .
WORKDIR "/src/Guids.Api.Client"
RUN dotnet build "Guids.Api.Client.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Guids.Api.Client.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Guids.Api.Client.dll", "http://guids.api:5000"]