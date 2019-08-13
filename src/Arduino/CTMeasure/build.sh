~/Temp/arduino-1.8.9/arduino --upload CTMeasure.ino --port /dev/ttyACM0
stty -F /dev/ttyACM0 115200 raw -clocal -echo
cat /dev/ttyACM0