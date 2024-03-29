FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
COPY ["WebApplicationAPI/WebApplicationAPI.csproj", "WebApplicationAPI/"]
RUN dotnet restore "WebApplicationAPI/WebApplicationAPI.csproj"
COPY . .
WORKDIR "/WebApplicationAPI"
RUN dotnet build "WebApplicationAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApplicationAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApplicationAPI.dll"]