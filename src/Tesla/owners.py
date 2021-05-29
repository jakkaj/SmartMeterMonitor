import asyncio
from tesla_api import TeslaApiClient
import json
from datetime import date
from datetime import datetime

async def save_token(token):
    open("token_file.json", "w").write(token)

async def load_token():
    return open("token_file.json", "r").read()

async def main():

    # reminder got this original token from postman auth thingo using authcode with pkce settings

    #o = {}
    #o["refresh_token"] = ""
    #o["access_token"] = ""
    #o["expires_in"]=28800
    #o["created_at"] = datetime.timestamp(datetime.now())
    #tokens = json.dumps(o)
    #await save_token(tokens)
    #exit()
    tokens = await load_token()    

    client = TeslaApiClient(token=tokens, on_new_token=save_token)

    energy_sites = await client.list_energy_sites()
    print("Number of energy sites = %d" % (len(energy_sites)))
    assert(len(energy_sites)==1)
    reserve = await energy_sites[0].get_backup_reserve_percent()
    print("Backup reserve percent = %d" % (reserve))
    print("Increment backup reserve percent")
    #await energy_sites[0].set_backup_reserve_percent(0)
    
    await client.close()

asyncio.run(main())