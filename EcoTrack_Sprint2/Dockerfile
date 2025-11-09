# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia tudo
COPY . .

# Restaura dependências do projeto principal
WORKDIR "/src/EcoTrack.Net"
RUN dotnet restore "EcoTrack.Net.csproj"

# Compila e publica
RUN dotnet publish "EcoTrack.Net.csproj" -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "EcoTrack.Net.dll"]
