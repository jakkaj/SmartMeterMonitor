
import json
import time
import sunspec.core.client as client
import time
import sys
import os
import json

from dotenv import load_dotenv
from tesla_powerwall import Powerwall
from tesla_powerwall import User

load_dotenv()

GW_USER=os.getenv("GW_USER")
GW_PASSWORD=os.getenv("GW_PASSWORD")
GW_IP=os.getenv("GW_IP")

# Create a simple powerwall object by providing the IP
powerwall = Powerwall(GW_IP)
#=> <Powerwall ...>




# Login as customer without email
# The default value for the email is ""
#powerwall.login("<password>")
#=> <LoginResponse ...>

# Login as customer with email
powerwall.login(GW_PASSWORD, GW_USER)
# Create a powerwall object with more options
#powerwall = Powerwall(
 #   endpoint="192.168.0.135",
    # Configure timeout; default is 10
 #   timeout=10,
    # Provide a requests.Session
  #  http_sesion=None,
    # Whether to verify the SSL certificate or not
   # verify_ssl=False,
   # disable_insecure_warning=True,
    # Set the API to expect a specific version of the powerwall software
   # pin_version=None
#)


# print(powerwall.is_authenticated())

charge=powerwall.get_charge()

print("Charge")
print(charge)


meters = powerwall.get_meters()
meters.charge=charge
#print(meters)



strm = str(meters.__dict__)
print(strm)


solar = meters.solar.instant_power
grid = meters.site.instant_power
battery = meters.battery.instant_power
load = meters.load.instant_power


print(solar)
print(grid)
print(battery)
print(load)


draw_solar = meters.solar.is_drawing_from()

sending_to_load = meters.load.is_sending_to()

battery_active = meters.battery.is_active()
battery_charge = meters.battery.is_sending_to()
battery_discharge = meters.battery.is_drawing_from()

print(battery_charge)
print(battery_discharge)

print(draw_solar)
print(sending_to_load)
print(battery_active)

operation_mode = powerwall.get_operation_mode()
#=> <OperationMode.SELF_CONSUMPTION: ...>
reserve_percentage = powerwall.get_backup_reserve_percentage()
#=> 5.000019999999999

print(operation_mode)

print(reserve_percentage)