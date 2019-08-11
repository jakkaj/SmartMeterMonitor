// EmonLibrary examples openenergymonitor.org, Licence GNU GPL V3

#include "EmonLib.h"             // Include Emon Library
EnergyMonitor emon1;             // Create an instance
#include <Wire.h>
#define VOLT_CAL 251.1
#define CUR_CAL 63.5

#define SLAVE_ADDRESS 0x08

int receiveBuffer[9];
uint8_t keepCounted = 0;

float realPower = 0;
float apparentPower = 0;
float powerFActor = 0;
float supplyVoltage = 0;
float Irms = 0;

void setup()
{  
  Serial.begin(115200);
  //Wire.setClock(5000);
  
  Wire.begin(SLAVE_ADDRESS);
  Wire.onReceive(receiveData);
  Wire.onRequest(sendData);

  emon1.voltage(1, VOLT_CAL, 1.7);  // Voltage: input pin, calibration, phase_shift
  emon1.current(0, CUR_CAL);       // Current: input pin, calibration.
}

void loop()
{
  emon1.calcVI(20,2000);         // Calculate all. No.of half wavelengths (crossings), time-out
  emon1.serialprint();           // Print out all variables (realpower, apparent power, Vrms, Irms, power factor)
  
   realPower       = emon1.realPower;        //extract Real Power into variable
   apparentPower   = emon1.apparentPower;    //extract Apparent Power into variable
   powerFActor     = emon1.powerFactor;      //extract Power Factor into Variable
   supplyVoltage   = emon1.Vrms;             //extract Vrms into Variable
   Irms            = emon1.Irms;             //extract Irms into Variable

  Serial.println(realPower);
  
  delay(2000);
}

// Read data in to buffer, offset in first element.
void receiveData(int byteCount){
  int counter = 0;
  while(Wire.available()) {
    receiveBuffer[counter] = Wire.read();
    //Serial.print("Got data: ");
    //Serial.println(receiveBuffer[counter]);
    counter ++;
  }
}
 
 
// Use the offset value to select a function
void sendData(){
  switch(receiveBuffer[0]){
    case 01:
      writeData(realPower);
      break;
    case 02:
      writeData(apparentPower);
      break;
    case 03:
      writeData(powerFActor);
      break;
    case 05:
      writeData(supplyVoltage);
      break;
    case 06: 
      writeData(Irms);
      break;
    default:
      Serial.println("No function for this address");
      break;
  }
  
  
}
 
 
void writeData(float newData) { 
  byte bytes_array[4];
  *((float *)bytes_array) = newData;
  
  int dataSize = sizeof(bytes_array);
  Wire.write(bytes_array, dataSize); 
}

// Counter function
int keepCount() {
  keepCounted ++;
  if (keepCounted > 255) {
    keepCounted = 0;
    return 0;
  } else {
    return keepCounted;
  }
}
