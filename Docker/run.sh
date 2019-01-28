#!/bin/bash

docker build -t jakkaj/smartmetermqttclient ../

set -o allexport
source .env
set +o allexport

docker-compose up -d
#dotnet watch --project ../MqttClient_Influx run 

#docker run -it -d -p 1883:1883 -p 9001:9001 -v eclipse-mosquitto eclipse-mosquitto
#docker run -t -d -p 80:5000 jakkaj/smartmeterapi
#docker run -t -d jakkaj/smartmetermqttclient