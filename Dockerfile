FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Guids.Api/Guids.Api.csproj", "Guids.Api/"]
COPY ["Guids.Data/Guids.Data.csproj", "Guids.Data/"]
RUN dotnet restore "Guids.Api/Guids.Api.csproj"
COPY . .
WORKDIR "/src/Guids.Api"
RUN dotnet build "Guids.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Guids.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Guids.Api.dll"]