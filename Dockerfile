FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
# Copy everything and build
COPY . ./
RUN dotnet restore "./KiancaAPI/KiancaAPI.sln"
RUN dotnet publish "./KiancaAPI/KiancaAPI.sln" -c Release -o out
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "KiancaAPI.dll"]