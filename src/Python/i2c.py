from time import sleep
from smbus2 import SMBusWrapper
from threading import Thread

import struct

class i2c_communications:
    address = 0x08

    #def __init__(self):
      
    #def begin_comms(self):
    #    thr = Thread(target=self.loop, args=[])
    #    thr.start()

    def getValue(self, value):
        
        try:
            with SMBusWrapper(1) as bus:
                
                data = bus.read_i2c_block_data(self.address, value, 4)
                b = ''.join(chr(i) for i in data)
                result = struct.unpack('f', b)
                self.weight=result
                # print(result)
                return result[0];                   

        except:
            print(' Oops! Error')                     
        
        return 0
