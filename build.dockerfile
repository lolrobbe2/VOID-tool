# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source/foxhole-void-bot

# Copy everything and build app
COPY foxhole-void-bot/. .
# copy everything and restore, build app
RUN dotnet publish -c release -o /app
# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "foxhole-void-bot.dll"]
