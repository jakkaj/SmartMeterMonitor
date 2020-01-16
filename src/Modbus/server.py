from flask import Flask
from flask import request
from flask import Response
import json
import time
app = Flask(__name__)

import sunspec.core.client as client



@app.route('/inverter')
def hello_world():   
    ip = request.args.get('ip')
    
    d = client.SunSpecClientDevice(client.TCP, 1,  ipaddr=ip, ipport=1502)
    
    time.sleep(0.5)
    d.inverter.read()
    d.ac_meter.read()
    d.close()
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
    dict["meter"] = dictac
    dumped = json.dumps(dict)
    #print(dumped)
    return Response(dumped, mimetype="application/json")

if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0')    