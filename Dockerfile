# Build
FROM mcr.microsoft.com/dotnet/sdk:11.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore ./DownloadApi/DownloadApi.csproj
RUN dotnet publish ./DownloadApi/DownloadApi.csproj -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:11.0 AS runtime

RUN apt-get update \
 && apt-get install -y libicu-dev \
 && rm -rf /var/lib/apt/lists/*

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "DownloadApi.dll"]