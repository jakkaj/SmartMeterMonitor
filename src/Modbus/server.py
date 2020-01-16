from flask import Flask
from flask import request
from flask import Response
import json
import time
from datetime import datetime
from datetime import timedelta 
app = Flask(__name__)

from seapi.solaredge import Solaredge
from dotenv import load_dotenv
import os


import sunspec.core.client as client


load_dotenv()





@app.route('/inverter')
def inverter():   
    
    SOLAREDGE_API_KEY = os.getenv("SOLAREDGE_API_KEY")
    SOLAREDGE_SITE_ID = os.getenv("SOLAREDGE_SITE_ID")

    se = Solaredge(SOLAREDGE_API_KEY)

    site_id = SOLAREDGE_SITE_ID
    ip = request.args.get('ip')
    
    d = client.SunSpecClientDevice(client.TCP, 1,  ipaddr=ip, ipport=1502)
    
    time.sleep(0.5)
    d.inverter.read()
    d.ac_meter.read()
    d.close()

    se_energy = se.get_energy(site_id, datetime.now().strftime("%Y-%m-%d"), (datetime.now() + timedelta(days=1)).strftime("%Y-%m-%d"), timeUnit='DAY')
    
    se = Solaredge(SOLAREDGE_API_KEY)

    site_id = SOLAREDGE_SITE_ID

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
    dict["seenergy"] = se_energy

    dumped = json.dumps(dict)
    #print(dumped)
    return Response(dumped, mimetype="application/json")

if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0')    