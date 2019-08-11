#!/usr/bin/python2.7
import paho.mqtt.client as mqtt
from i2c import i2c_communications
from time import sleep
import numpy

comms = i2c_communications()

def on_connect(client, userdata, flags, rc):
        print("Connected with result code "+str(rc))

        # Subscribing in on_connect() means that if we lose the connection and
        # reconnect then subscriptions will be renewed.
        #client.subscribe("$SYS/#")
        print("Connected")
    
    # The callback for when a PUBLISH message is received from the server.
def on_message(client, userdata, msg):
        print("JK" + msg.topic+" "+str(msg.payload))

def loop():
    
   
    print("i2ctest: Running")

    while 1:
        #print("something")
        sleep(2)
        power = comms.getValue(01)
        voltage = comms.getValue(05)
        print("Power: " + str(power))
        print("Volts: " + str(voltage))
        client.publish("f_volts", voltage)
        client.publish("f_power", power)
        
client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_message

client.connect_async("192.168.0.220", 1883)
client.loop_start()
loop()