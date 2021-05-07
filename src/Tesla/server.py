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

import logging
logging.basicConfig(level=logging.DEBUG)

load_dotenv()


GW_USER=os.getenv("GW_USER")
GW_PASSWORD=os.getenv("GW_PASSWORD")
GW_IP=os.getenv("GW_IP")


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