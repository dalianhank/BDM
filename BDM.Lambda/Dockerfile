#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BDM.Lambda/BDM.Lambda.csproj", "BDM.Lambda/"]
COPY ["BDM.Data/BDM.Data.csproj", "BDM.Data/"]
RUN dotnet restore "BDM.Lambda/BDM.Lambda.csproj"
COPY . .
WORKDIR "/src/BDM.Lambda"
RUN dotnet build "BDM.Lambda.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BDM.Lambda.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BDM.Lambda.dll"]
