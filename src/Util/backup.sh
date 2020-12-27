#!/bin/bash
#cron 0 0 */2 * * /home/jordan/GitHub/SmartMeterMonitor3/src/Util/backup.sh > /home/jordan/cron.log 2>&1

. /home/pi/.bashrc
container=$(docker ps | grep "influxdb" | awk '{ print $1 }')
date=$(date -Is)
dir=/data/influx2/$date
mkdir $dir
command="influxd backup -portable -host 127.0.0.1:8088 ./var/lib/influxdb/$date"
/usr/bin/docker exec $container $command
zip -r /home/pi/backup/influx/$date.zip $dir
rclone copy /home/pi/backup/influx/$date.zip onedrive:backup/house
rm /home/pi/backup/influx/$date.zip
rm -r $dir  