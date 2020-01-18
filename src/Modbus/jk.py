
import json
import time
import sunspec.core.client as client
import time
import sys


d = client.SunSpecClientDevice(client.TCP, 1,  ipaddr="192.168.0.107", ipport=1502)
time.sleep(0.5)
#print (d.models)
#print d.inverter.points

now = time.time()
time.sleep(2)
print(time.time() - now)

while True:

   # d = client.SunSpecClientDevice(client.TCP, 1, ipaddr="192.168.0.107", ipport=1502)
    
    d.inverter.read()
    d.ac_meter.read()
    
    #dumped = json.dumps(d)
    #print(dumped)
    dict = {}
    for (key, value) in d.inverter.model.__dict__.items():
        #print(key)
        if(key == "points"):
            
            for(k, v) in value.items():
                #print(k)
                #print(v.value_base)
                dict[k] = d.inverter[k]
            
        #print(str(value))
    dictac = {}
    for (key, value) in d.ac_meter.model.__dict__.items():
        #print(key)
        if(key == "points"):
            
            for(k, v) in value.items():
                #print(k)
                #print(v.value_base)
                dictac[k] = d.ac_meter[k]
            
        #print(str(value))
    
    dumped = json.dumps(dictac)
    print(dumped)
    #print export
    #print d.inverter["W"]
    #print d.inverter["PhVphA"]
    #print d.inverter["VAr"]
    time.sleep(5)