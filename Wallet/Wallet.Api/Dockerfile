FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["Wallet.Api/Wallet.Api.csproj", "Wallet.Api/"]

RUN dotnet restore "Wallet.Api/Wallet.Api.csproj"
COPY . .
WORKDIR "/src/Wallet.Api"
RUN dotnet build "Wallet.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Wallet.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Wallet.Api.dll"]