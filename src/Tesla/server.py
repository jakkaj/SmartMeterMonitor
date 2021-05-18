from flask import Flask
from flask import request
from flask import Response
import json
import time
from datetime import datetime
from datetime import timedelta 
app = Flask(__name__)

from dotenv import load_dotenv
import os

from tesla_powerwall import Powerwall
from tesla_powerwall import User

import asyncio
from tesla_api import TeslaApiClient

import logging
logging.basicConfig(level=logging.DEBUG)

load_dotenv()


GW_USER=os.getenv("GW_USER")
GW_PASSWORD=os.getenv("GW_PASSWORD")
GW_IP=os.getenv("GW_IP")
TESLA_USER=os.getenv("TESLA_USER")
TESLA_PASSWORD=os.getenv("TESLA_PASSWORD")

loop = asyncio.get_event_loop()


async def getreserve():
    client = TeslaApiClient(TESLA_USER, TESLA_PASSWORD) 
    energy_sites = await client.list_energy_sites()
    assert(len(energy_sites)==1)   
    reserve = await energy_sites[0].get_backup_reserve_percent()    
    await client.close()
    print(reserve)
    return reserve


async def setreserve():
    client = TeslaApiClient(TESLA_USER, TESLA_PASSWORD)        
    energy_sites = await client.list_energy_sites()    
    assert(len(energy_sites)==1)
    print("Increment backup reserve percent")
    await energy_sites[0].set_backup_reserve_percent(0)    
    await client.close()
    return "OK"

@app.route('/reserve', methods=['GET'])
def reserve_get():
    percent = request.args.get('percent')
    result = loop.run_until_complete(getreserve())
    return str(result)


@app.route('/reserve', methods=['PUT'])
def reserve_set():
    percent = request.args.get('percent')
    return loop.run_until_complete(setreserve())    

@app.route('/powerwall')
def powerwall():   
    
    strm=""
    try:
        powerwall = Powerwall(GW_IP)   
        
        powerwall.login(GW_PASSWORD, GW_USER)

        charge=powerwall.get_charge()
        meters = powerwall.get_meters()
        meters.charge=charge
        strm = str(meters.__dict__)
    except Exception as ex:
        print("Could not access Tesla Gateway: "+ str(ex))
        return Response(str(ex), mimetype="application/json")

    return Response(strm, mimetype="application/json")

if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port='5001')    