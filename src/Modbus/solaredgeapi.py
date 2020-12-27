import json
from datetime import datetime
from datetime import timedelta 
import sunspec.core.client as client
import time
import sys
from seapi.solaredge import Solaredge
from dotenv import load_dotenv
import os

load_dotenv()

SOLAREDGE_API_KEY = os.getenv("SOLAREDGE_API_KEY")
SOLAREDGE_SITE_ID = os.getenv("SOLAREDGE_SITE_ID")


se = Solaredge(SOLAREDGE_API_KEY)

site_id = SOLAREDGE_SITE_ID

#result = se.get_details(1454975)
#result = se.get_timeFrameEnergy(site_id, datetime.now().strftime("%Y-%m-%d"), (datetime.now() + timedelta(days=1)).strftime("%Y-%m-%d"), timeUnit='DAY')
result = se.get_currentPowerFlow(site_id)
#print(result)
result2 = json.dumps(result)

#dict = json.loads(result)
print(result2)