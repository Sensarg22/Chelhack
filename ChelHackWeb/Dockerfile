FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ChelHackWeb/ChelHackWeb.csproj ChelHackWeb/
RUN dotnet restore ChelHackWeb/ChelHackWeb.csproj
COPY . .
WORKDIR /src/ChelHackWeb
RUN dotnet build ChelHackWeb.csproj -c Release -o /app

FROM node:10-alpine as npm
ARG VERSION
WORKDIR /src
COPY --from=build "/ChelHackWeb/app" .
RUN npm install && npm run build

FROM build AS publish
WORKDIR /src/ChelHackWeb
COPY --from=npm "/src/dist" "App/dist"
RUN dotnet publish ChelHackWeb.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ChelHackWeb.dll"]
