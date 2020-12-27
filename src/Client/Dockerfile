FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers

COPY ./EnergyHost.App/EnergyHost.App.csproj ./EnergyHost.App/
COPY ./EnergyHost.Contract/EnergyHost.Contract.csproj ./EnergyHost.Contract/
COPY ./EnergyHost.Model/EnergyHost.Model.csproj ./EnergyHost.Model/
COPY ./EnergyHost.Services/EnergyHost.Services.csproj ./EnergyHost.Services/
COPY ./EnergyHost.Tests/EnergyHost.Tests.csproj ./EnergyHost.Tests/
COPY ./NetatmoCore/NetatmoCore/NetatmoCore.csproj ./NetatmoCore/NetatmoCore

RUN dotnet restore ./EnergyHost.App/


# Copy everything else and build
COPY . ./

RUN dotnet publish -c Debug -o /app/out ./EnergyHost.App/

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
ENV TZ=Australia/Sydney
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 5000
ENTRYPOINT ["dotnet", "EnergyHost.App.dll"]