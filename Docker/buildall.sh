#!/bin/bash

#docker build -t jakkaj/smartmeterqueue .
docker build -t jakkaj/smartmeterapi ../web/SmartMeterService
docker build -t jakkaj/smartmetermqttclient ../MqttClient