#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

WORKDIR /src

COPY *.csproj ./

RUN dotnet restore   "CosmosGettingStartedDotnetCoreTutorial.csproj"

COPY . .

WORKDIR "/src/"

RUN dotnet build "CosmosGettingStartedDotnetCoreTutorial.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish  "CosmosGettingStartedDotnetCoreTutorial.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# copy the SSL certificate
COPY cosmosEmulatorCert.crt /usr/share/ca-certificates
RUN echo cosmosEmulatorCert.crt >> /etc/ca-certificates.conf
RUN update-ca-certificates

ENTRYPOINT ["dotnet", "CosmosGettingStartedDotnetCoreTutorial.dll"]

