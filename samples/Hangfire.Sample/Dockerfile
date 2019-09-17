FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Hangfire.JobExtensions/Hangfire.JobExtensions.csproj", "Hangfire.JobExtensions/"]
RUN dotnet restore "Hangfire.JobExtensions/Hangfire.JobExtensions.csproj"
COPY . .
WORKDIR "/src/Hangfire.JobExtensions"
RUN dotnet build "Hangfire.JobExtensions.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Hangfire.JobExtensions.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Hangfire.JobExtensions.dll"]