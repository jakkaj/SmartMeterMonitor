#!/bin/bash
#cron 0 0 */2 * * /home/jak/GitHub/SmartMeterMonitor/src/Util/backup.sh > /home/jak/cron.log 2>&1

. /home/jak/.bashrc
container=$(docker ps | grep "influxdb" | awk '{ print $1 }')
date=$(date -Is)
dir=/data/influx1/$date
mkdir $dir
command="influxd backup -portable -host 127.0.0.1:8088 ./var/lib/influxdb/$date"
/usr/bin/docker exec $container $command
cp /data/grafana/grafana.db $dir/grafana.db
zip -r /home/jak/backup/influx/$date.zip $dir
rclone copy /home/jak/backup/influx/$date.zip onedrive:backup/house
rm /home/jak/backup/influx/$date.zip
rm -r $dir  
