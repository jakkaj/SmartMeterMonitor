

#include "HttpClient.h"
#include "QueueClient.h"
#include "Statistic.h"

#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>

#include <PubSubClient.h>

#include <Wire.h>

const char *ssid = "TelstraCAC61A";
const char *password = "mm2mdkbfrn";
const char *mqtt_server = "192.168.0.220";

Statistic stats;

char msg[50];

HttpClient httpClient(ssid, password, true);
QueueClient queueClient(&httpClient, mqtt_server);
const int sclPin = D1;
const int sdaPin = D2;
void setup()
{
  
  //Wire.setClock(5000);
  Wire.begin(sdaPin, sclPin, 8); // join i2c bus with address #8
  Wire.onReceive(receiveEvent);

  Serial.begin(115200);

  httpClient.init();

  delay(200);

  queueClient.setEnabled(true);

  queueClient.sendQueue("log", "Booted");
}

void loop()
{
  delay(100);
}

void receiveEvent(int howMany)
{

  String receive = "";
  Serial.println("Receive");
  while (Wire.available())
  {                       // loop through all but the last
    char c = Wire.read(); // receive byte as a character
    receive += c;
  }
  
  Serial.println(receive);

  // Length (with one extra character for the null terminator)
  int str_len = receive.length() + 1; 
 
  // Prepare the character array (the buffer) 
  char char_array[str_len];

  receive.toCharArray(char_array, str_len);

  queueClient.sendQueue("ct", char_array);
}
