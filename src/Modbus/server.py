from flask import Flask
from flask import request
app = Flask(__name__)

import sunspec.core.client as client


@app.route('/inverter')
def hello_world():   
    ip = request.args.get('ip')
    field = request.args.get('field')
    d = client.SunSpecClientDevice(client.TCP, 2, ipaddr=ip)
    d.inverter.read()
    return  str(d.inverter[field])

if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0')
    