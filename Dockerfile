FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["everest-common/everest-common.csproj", "."]
RUN dotnet restore "everest-common.csproj"
COPY everest-common/. .

COPY ["Everest/everest-app/everest-app.csproj", "."]
RUN dotnet restore "everest-app.csproj"
COPY Everest/everest-app/. .
RUN dotnet build "everest-app.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "everest-app.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "everest-app.dll"]
