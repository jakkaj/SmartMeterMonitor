
import json
import sunspec.core.client as client
import time


d = client.SunSpecClientDevice(client.TCP, 2, ipaddr="192.168.0.201")

print (d.models)
print d.inverter.points

while True:

    d.inverter.read()
    export = json.dumps(d.inverter)
    print export
    print d.inverter["W"]
    print d.inverter["PhVphA"]
    print d.inverter["VAr"]
    time.sleep(5)