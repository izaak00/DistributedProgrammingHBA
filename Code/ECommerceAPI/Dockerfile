FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY Shipping/Shipping.csproj Shipping/
RUN dotnet restore Shipping/Shipping.csproj

COPY . ./
RUN dotnet publish Shipping -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Shipping.dll"]