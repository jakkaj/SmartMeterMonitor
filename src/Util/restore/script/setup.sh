mkdir /var/lib/influxdb/temp


#unzip -o /var/lib/influxdb/influx8aug.zip -d /var/lib/influxdb/temp/

container=$(docker ps | grep "influxdb" | awk '{ print $1 }')

command="influxd restore -portable  -db house -newdb house_bak /var/lib/influxdb/temp/data/influx2/2021-08-08T00:00:02+10:00"
/usr/bin/docker exec $container $command

#rm -rf /var/lib/influxdb/temp