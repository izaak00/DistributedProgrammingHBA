FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY ECommerce/ECommerce.csproj ECommerce/
RUN dotnet restore ECommerce/ECommerce.csproj

COPY . ./
RUN dotnet publish ECommerce -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "ECommerce.dll"]