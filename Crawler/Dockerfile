FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Crawler/Crawler.csproj Crawler/
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Common/Common.csproj", "Common/"]
RUN dotnet restore Crawler/Crawler.csproj
COPY . .
WORKDIR /src/Crawler
RUN dotnet build Crawler.csproj -c Debug -o /app

FROM build AS publish
RUN dotnet publish Crawler.csproj -c Debug -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Crawler.dll"]
