FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ankh.Backend/Ankh.Backend.csproj", "Ankh.Backend/"]
COPY ["Ankh/Ankh.csproj", "Ankh/"]
RUN dotnet restore "Ankh.Backend/Ankh.Backend.csproj"
COPY . .
WORKDIR "/src/Ankh.Backend"
RUN dotnet build "Ankh.Backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ankh.Backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ankh.Backend.dll"]
