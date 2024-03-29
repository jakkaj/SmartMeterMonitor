
#ifndef __WEMOS_DHT12_H
#define __WEMOS_DHT12_H


#if ARDUINO >= 100
 #include "Arduino.h"
#else
 #include "WProgram.h"
#endif

#include "Wire.h"

class DHT12{
public:
	DHT12(uint8_t address=0x5c);
	byte get(void);
	float cTemp=0;
	float fTemp=0;
	float humidity=0;

private:
	uint8_t _address;

};


#endif

