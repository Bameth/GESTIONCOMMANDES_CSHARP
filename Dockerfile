# Étape 1 : Construire l'application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copier le fichier projet et restaurer les dépendances
COPY *.csproj ./
RUN dotnet restore

# Copier tout le projet et compiler
COPY . ./
RUN dotnet publish -c Release -o /out

# Étape 2 : Utiliser une image plus légère pour exécuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

# Exposer le port
EXPOSE 80
EXPOSE 443

# Démarrer l'application
ENTRYPOINT ["dotnet", "GESTIONCOMMANDES.dll"]
