FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PinBot/PinBot.csproj", "PinBot/"]
RUN dotnet restore "PinBot/PinBot.csproj"
COPY . .
WORKDIR "/src/PinBot"
RUN dotnet build "PinBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PinBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["/app/PinBot"]
