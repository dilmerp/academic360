# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copiar los csproj respetando la carpeta src/ (para restaurar dependencias primero)
COPY ["src/Academic360.Api/Academic360.Api.csproj", "src/Academic360.Api/"]
COPY ["src/Academic360.Application/Academic360.Application.csproj", "src/Academic360.Application/"]
COPY ["src/Academic360.Domain/Academic360.Domain.csproj", "src/Academic360.Domain/"]
COPY ["src/Academic360.Infrastructure/Academic360.Infrastructure.csproj", "src/Academic360.Infrastructure/"]

# Restaurar dependencias desde el proyecto principal
RUN dotnet restore "src/Academic360.Api/Academic360.Api.csproj"

# Copiar todo el resto del código fuente
COPY . .

# Moverse a la carpeta del API y publicar
WORKDIR "/app/src/Academic360.Api"
RUN dotnet publish "Academic360.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render usa el puerto 8080 por defecto
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Academic360.Api.dll"]
