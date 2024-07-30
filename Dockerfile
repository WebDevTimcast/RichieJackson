FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim-amd64 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ON.Install ON.Install
COPY SimpleWeb SimpleWeb
WORKDIR "/src/SimpleWeb/"
RUN dotnet restore "SimpleWeb.csproj"
RUN dotnet build "SimpleWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleWeb.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleWeb.dll"]