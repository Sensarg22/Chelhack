FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ChalHackApi/ChalHackApi.csproj ChalHackApi/
RUN dotnet restore ChalHackApi/ChalHackApi.csproj
COPY . .
WORKDIR /src/ChalHackApi
RUN dotnet build ChalHackApi.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ChalHackApi.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ChalHackApi.dll"]
