
import json
import sunspec.core.client as client
import time
import sys



#print (d.models)
#print d.inverter.points

while True:

   # d = client.SunSpecClientDevice(client.TCP, 1, ipaddr="192.168.0.107", ipport=1502)
    
    try:
        d = client.SunSpecClientDevice(client.TCP, 1, ipaddr="192.168.0.107", ipport=1502)
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
        break
    except:
        print "Unexpected error:", sys.exc_info()[0]
    time.sleep(5)