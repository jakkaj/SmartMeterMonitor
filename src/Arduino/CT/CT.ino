#include "EmonLib.h"
// Include Emon Library
EnergyMonitor emon1;
// Create an instance

#include "HttpClient.h"
#include "QueueClient.h"
#include "Statistic.h"

#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>

#include <PubSubClient.h>


const char *ssid = "TelstraCAC61A";
const char *password = "mm2mdkbfrn";
const char *mqtt_server = "192.168.0.220";

Statistic stats;

char msg[50];

HttpClient httpClient(ssid, password, true);
QueueClient queueClient(&httpClient, mqtt_server);


#define VOLT_CAL 242.2
#define CURRENT_CAL 112

bool measuring = true;

void setup()
{
  
  Serial.begin(115200);

  httpClient.init();

  delay(200);

  queueClient.setEnabled(true);

  queueClient.sendQueue("log", "Booted");
  
  emon1.current(0, CURRENT_CAL);             // Current: input pin, calibration.
}

void loop()
{
  stats.clear();
  while(stats.count() < 20){
    double Irms = emon1.calcIrms(1480);
    //Serial.println(Irms);
    stats.add(Irms);
    delay(10);    
  }

  float avg  = stats.average();
  float current = avg * VOLT_CAL;
  //double Irms = emon1.calcIrms(1480);  // Calculate Irms only
  Serial.println(current);

  snprintf(msg, 50, "%ld", avg);
  
  queueClient.sendQueue("ctirms", msg);

  snprintf(msg, 50, "%ld", current);
  queueClient.sendQueue("ctwatts", msg);
  //Serial.println(msg);


  //Serial.print(" ");
//  Serial.println(Irms);             // Irms

  delay(200);
}
