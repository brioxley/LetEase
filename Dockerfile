FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LetEase.API/LetEase.API.csproj", "LetEase.API/"]
COPY ["LetEase.Core/LetEase.Core.csproj", "LetEase.Core/"]
COPY ["LetEase.Infrastructure/LetEase.Infrastructure.csproj", "LetEase.Infrastructure/"]
RUN dotnet restore "LetEase.API/LetEase.API.csproj"
COPY . .
WORKDIR "/src/LetEase.API"
RUN dotnet build "LetEase.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LetEase.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LetEase.API.dll"]