import asyncio
from tesla_api import TeslaApiClient

async def main():
    client = TeslaApiClient('j@mail.com', '12345')

    energy_sites = await client.list_energy_sites()
    print("Number of energy sites = %d" % (len(energy_sites)))
    assert(len(energy_sites)==1)
    reserve = await energy_sites[0].get_backup_reserve_percent()
    print("Backup reserve percent = %d" % (reserve))
    print("Increment backup reserve percent")
    await energy_sites[0].set_backup_reserve_percent(0)
    
    await client.close()

asyncio.run(main())