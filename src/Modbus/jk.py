
import json
import sunspec.core.client as client
import time


d = client.SunSpecClientDevice(client.TCP, 2, ipaddr="192.168.0.201")

#print (d.models)
#print d.inverter.points

while True:


    
    
    d.inverter.read()
    dict = {}
    for (key, value) in d.inverter.model.__dict__.items():
        #print(key)
        if(key == "points"):
            
            for(k, v) in value.items():
                #print(k)
                #print(v.value_base)
                dict[k] = d.inverter[k]
            
        #print(str(value))
    dumped = json.dumps(dict)
    print(dumped)
    #print export
    #print d.inverter["W"]
    print d.inverter["PhVphA"]
    #print d.inverter["VAr"]
    time.sleep(5)