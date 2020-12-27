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

import logging
logging.basicConfig(level=logging.DEBUG)

load_dotenv()


date_since = time.time()

se_energy = None

se_currentPower = None

@app.route('/inverter')
def inverter():   
    global date_since
    global se_energy
    global se_currentPower
    SOLAREDGE_API_KEY = os.getenv("SOLAREDGE_API_KEY")
    SOLAREDGE_SITE_ID = os.getenv("SOLAREDGE_SITE_ID")

    se = Solaredge(SOLAREDGE_API_KEY)

    site_id = SOLAREDGE_SITE_ID
    ip = request.args.get('ip')
    
    

    if(time.time() - date_since > 900 or se_energy is None or se_currentPower is None):
        date_since = time.time()
        try:
            se_energy = se.get_energyDetails(site_id, datetime.today().strftime("%Y-%m-%d 00:00:00"), (datetime.today()).strftime("%Y-%m-%d 23:59:59"))
            se_currentPower = se.get_currentPowerFlow(site_id)
            print("SolarEdge API successful")
        except Exception as ex:
            print("Could not access SolarEdge API: "+ str(ex))
    
    dict = {}

    try:
        d = client.SunSpecClientDevice(client.TCP, 1,  ipaddr=ip, ipport=1502)
        
        time.sleep(0.5)
        d.inverter.read()
        d.ac_meter.read()
        d.close()    

        
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
    except Exception as ex:
            print("Could not access modbus: "+ str(ex))
    
    if not se_energy is None:
        dict.update(se_energy)
    if not se_currentPower is None:
        dict.update(se_currentPower)

    dumped = json.dumps(dict)
    #print(dumped)
    return Response(dumped, mimetype="application/json")

if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0')    