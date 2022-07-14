#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GFA.Medicals.Server.Api/GFA.Medicals.Server.Api.csproj", "GFA.Medicals.Server.Api/"]
RUN dotnet restore "GFA.Medicals.Server.Api/GFA.Medicals.Server.Api.csproj"
COPY . .
WORKDIR "/src/GFA.Medicals.Server.Api"
RUN dotnet build "GFA.Medicals.Server.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GFA.Medicals.Server.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GFA.Medicals.Server.Api.dll"]