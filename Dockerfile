# Use the .NET SDK image for Windows
FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2019 AS build
WORKDIR /src
COPY ["LetEase.API/LetEase.API.csproj", "LetEase.API/"]
COPY ["LetEase.Application/LetEase.Application.csproj", "LetEase.Application/"]
COPY ["LetEase.Infrastructure/LetEase.Infrastructure.csproj", "LetEase.Infrastructure/"]
COPY ["LetEase.Domain/LetEase.Domain.csproj", "LetEase.Domain/"]
RUN dotnet restore "LetEase.API/LetEase.API.csproj"
COPY . .
WORKDIR "/src/LetEase.API"
RUN dotnet build "LetEase.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LetEase.API.csproj" -c Release -o /app/publish

# Use the ASP.NET runtime image for Windows
FROM mcr.microsoft.com/dotnet/aspnet:8.0-windowsservercore-ltsc2019 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LetEase.API.dll"]