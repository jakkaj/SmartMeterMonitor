FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers

COPY ./EnergyHost.App/EnergyHost.App.csproj ./EnergyHost.App/
COPY ./EnergyHost.Contract/EnergyHost.Contract.csproj ./EnergyHost.Contract/
COPY ./EnergyHost.Model/EnergyHost.Model.csproj ./EnergyHost.Model/
COPY ./EnergyHost.Services/EnergyHost.Services.csproj ./EnergyHost.Services/
COPY ./EnergyHost.Tests/EnergyHost.Tests.csproj ./EnergyHost.Tests/

RUN dotnet restore ./EnergyHost.App/


# Copy everything else and build
COPY . ./

RUN dotnet publish -c Debug -o /app/out ./EnergyHost.App/

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 5000
ENTRYPOINT ["dotnet", "EnergyHost.App.dll"]