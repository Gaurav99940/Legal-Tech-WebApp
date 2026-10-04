# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["LT/LT.csproj", "LT/"]
COPY ["LT.Data/LT.Data.csproj", "LT.Data/"]
COPY ["LT.Model/LT.Model.csproj", "LT.Model/"]
COPY ["LT.Services/LT.Services.csproj", "LT.Services/"]
COPY ["LT.Utilities/LT.Utilities.csproj", "LT.Utilities/"]

# Restore dependencies
RUN dotnet restore "LT/LT.csproj"

# Copy full source and build
COPY . .
WORKDIR "/src/LT"
RUN dotnet build "LT.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "LT.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LT.dll"]
