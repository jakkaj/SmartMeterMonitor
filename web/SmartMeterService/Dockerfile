FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY SmartMeterService.sln ./
COPY Smart.Web/*.csproj ./Smart.Web/
COPY Smart.Helpers/*.csproj ./Smart.Helpers/
COPY Smart.Tests/*.csproj ./Smart.Tests/

RUN dotnet restore

# Copy everything else and build
COPY . ./
WORKDIR /app/Smart.Web
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 5000
ENTRYPOINT ["dotnet", "Smart.Web.dll"]