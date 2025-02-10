# Étape de build : utiliser le SDK pour compiler et publier l’application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copier le fichier projet et restaurer les packages
COPY *.csproj ./
RUN dotnet restore

# Copier l’ensemble du projet et publier en Release
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Étape finale : image runtime plus légère
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

# Exposer le port 80 (HTTP) et 443 (HTTPS) si besoin
EXPOSE 80
EXPOSE 443

# Lancer l’application
ENTRYPOINT ["dotnet", "GESTIONCOMMANDES.dll"]
