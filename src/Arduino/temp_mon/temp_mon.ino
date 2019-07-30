#include "WEMOS_DHT12.h"
#include "HttpClient.h"
#include "QueueClient.h"

#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>

#include <PubSubClient.h>
//const char* ssid = "CMO2";
//const char* password = "HellaAwesome2";

const char *ssid = "";
const char *password = "";
const char *mqtt_server = "192.168.0.220";
DHT12 dht12;

char msg[50];

HttpClient httpClient(ssid, password, true);
QueueClient queueClient(&httpClient, mqtt_server);



const int sleepSeconds = 10;

void setup()
{

  Serial.begin(115200);

  httpClient.init();

  delay(200);

  queueClient.setEnabled(true);

  queueClient.sendQueue("log", "Booted");

  

  pinMode(D0, WAKEUP_PULLUP);

  if (dht12.get() == 0)
  {
    Serial.print("Temperature in Celsius : ");
    Serial.println(dht12.cTemp);

    Serial.print("Temperature in Fahrenheit : ");
    Serial.println(dht12.fTemp);
    Serial.print("Relative Humidity : ");
    Serial.println(dht12.humidity);
    Serial.println();

    snprintf(msg, 50, "%ld", (int)dht12.cTemp);
    Serial.println(msg);
    queueClient.sendQueue("temp1", msg);

    snprintf(msg, 50, "%ld", (int)dht12.humidity);
    queueClient.sendQueue("humid1", msg);
  }

  ESP.deepSleep(sleepSeconds * 1000000);

}

void loop()
{

  
}