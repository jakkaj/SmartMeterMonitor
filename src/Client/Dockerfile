FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers

COPY ./MqttClient_Influx/MqttTestClient.csproj ./MqttClient_Influx/
COPY ./OpenWeatherMap/OpenWeatherMap.csproj ./OpenWeatherMap/

RUN dotnet restore ./MqttClient_Influx/
RUN dotnet restore ./OpenWeatherMap/

# Copy everything else and build
COPY . ./

RUN dotnet publish -c Debug -o /app/out ./MqttClient_Influx/

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 5000
ENTRYPOINT ["dotnet", "MqttTestClient.dll"]