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
result = se.get_energy(site_id, datetime.now().strftime("%Y-%m-%d"), (datetime.now() + timedelta(days=1)).strftime("%Y-%m-%d"), timeUnit='DAY')
print(result)

#dict = json.loads(result)
print(type(result))