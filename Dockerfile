# Étape 1 : Image officielle de .NET pour exécution
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Étape 2 : Image SDK pour la compilation
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Optimisation : copier uniquement le fichier projet et restaurer les dépendances
COPY ["GESTIONCOMMANDES.csproj", "./"]
RUN dotnet restore "GESTIONCOMMANDES.csproj"

# Copier tout le reste du projet et compiler
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Étape 3 : Image finale pour exécution
FROM base AS final
WORKDIR /app

# Copier les fichiers publiés
COPY --from=build /app/publish .

# Variables d'environnement (à adapter si besoin)
ENV ASPNETCORE_URLS="http://+:80"
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Commande d'exécution
ENTRYPOINT ["dotnet", "GESTIONCOMMANDES.dll"]

# Optionnel : Vérifier si l'app tourne bien
HEALTHCHECK --interval=30s --timeout=10s --start-period=10s \
  CMD curl -f http://localhost:80/ || exit 1
