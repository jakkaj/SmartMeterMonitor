#!/bin/bash


. /home/jordan/.bashrc
container=$(docker ps | grep "influxdb" | awk '{ print $1 }')
date=$(date -Is)
dir=/data/influx2/$date
mkdir $dir
command="influxd backup -portable -host 127.0.0.1:8088 ./var/lib/influxdb/$date"
/usr/bin/docker exec $container $command
zip -r /home/jordan/backup/influx/$date.zip $dir
rm -r $dir

