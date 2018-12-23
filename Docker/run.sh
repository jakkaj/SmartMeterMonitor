#!/bin/bash
docker run -it -p 1883:1883 -p 9001:9001 -v eclipse-mosquitto jakkaj/mqtt