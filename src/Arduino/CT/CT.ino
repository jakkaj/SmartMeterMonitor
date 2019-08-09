#include "EmonLib.h"
// Include Emon Library
EnergyMonitor emon1;
// Create an instance

#define VOLT_CAL 240
#define CURRENT_CAL 115
void setup()
{
  Serial.begin(9600);
  
  emon1.current(0, CURRENT_CAL);             // Current: input pin, calibration.
}

void loop()
{


double Irms = emon1.calcIrms(1480);  // Calculate Irms only
Serial.println(Irms*VOLT_CAL);           // Apparent power
  //Serial.print(" ");
//  Serial.println(Irms);             // Irms

  delay(200);
}
