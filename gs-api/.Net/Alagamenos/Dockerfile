# Etapa de build com .NET 9.0
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY ./ ./
RUN dotnet publish -c Release -o out

# Etapa de runtime com .NET 9.0
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Cria usuário não-root
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser

# Ajusta permissões da pasta de trabalho
RUN chown -R appuser:appgroup /app

# Muda para o usuário não-root
USER appuser

EXPOSE 80
ENTRYPOINT ["dotnet", "Alagamenos.dll"]
