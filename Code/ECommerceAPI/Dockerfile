FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY ProductCatalogue/ProductCatalogue.csproj ProductCatalogue/
RUN dotnet restore ProductCatalogue/ProductCatalogue.csproj

COPY . ./
RUN dotnet publish ProductCatalogue -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "ProductCatalogue.dll"]