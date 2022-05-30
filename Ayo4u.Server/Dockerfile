#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Ayo4u.Server/Ayo4u.Server.csproj", "Ayo4u.Server/"]
RUN dotnet restore "Ayo4u.Server/Ayo4u.Server.csproj"
COPY . .
WORKDIR "/src/Ayo4u.Server"
RUN dotnet build "Ayo4u.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ayo4u.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ayo4u.Server.dll"]