#include <elapsedMillis.h>
#include "Statistic.h"

#include "HttpClient.h"
#include "QueueClient.h"

#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>

#include <PubSubClient.h>
//const char* ssid = "CMO2";
//const char* password = "HellaAwesome2";

const char *ssid = "TelstraAE6511";
const char *password = "bpjzhvssks";
const char *mqtt_server = "10.0.0.59";

const boolean wifiAlwaysOn = true;
const boolean everyPulse = true;

HttpClient httpClient(ssid, password, wifiAlwaysOn);
QueueClient queueClient(mqtt_server);

elapsedMillis timeElapsed;
elapsedMillis thisPulse;
elapsedMillis pulsePeriod;

Statistic stats;

int impressionsCounted = 0;
int lastPeriod = 0;
int resetCounter = 0;

char msg[50];

bool wasLow = false;

bool isCounting = false;

void setup()
{

  Serial.begin(115200);
  httpClient.wifiOff();
  delay(1);

  httpClient.post("http://10.0.0.38:5000/impress/boot");
  
  log("Booted");
}

void loop()
{
  delay(10);
  // put your main code here, to run repeatedly:
  int val = analogRead(0);
  if (!wasLow)
  {
    stats.add(val);
  }

  if (stats.count() < 100)
  {
    return;
  }

  float avg = stats.average();

  //when we don't know the blink level yet
  //blink event will lower the val by some margin we don't yet know
  boolean isLow = false;

  float avgPercent = avg * 20 / 100;

  if (val < avg - avgPercent)
  {
    isLow = true;
  }

  if (isLow && !wasLow)
  {
    //this is a new pulse
    pulseStart();
  }
  else if (isLow && wasLow)
  {
    //still low
    wasLow = true;
  }
  else if (!isLow && wasLow)
  {
    //a finishing pulse
    pulseEnd();
    delay(400); //we can delay a bit to save power as there is no chance of action just now
  }
  else
  {
    //nothing
  }

  if (timeElapsed / 1000 > 30)
  {
    stats.clear();
    timeElapsed = 0;
    send();
  }
}

void send()
{
  Serial.print("Sending impressions:");
  Serial.print(impressionsCounted);
  Serial.println();

  String url = "http://10.0.0.38:5000/impress?time=30&imp=";
  String sendUrl = url + impressionsCounted;
  httpClient.post(sendUrl);

  snprintf(msg, 50, "%ld", impressionsCounted);
  queueClient.sendQueue("impressions", msg);

  lastPeriod = impressionsCounted;

  impressionsCounted = 0;
  isCounting = false;
  Serial.println("Sending done");
}

void pulseStart()
{
  if (pulsePeriod > 0)
  {
    Serial.println("Time between");
    Serial.println(pulsePeriod);
  }

  wasLow = true;
  thisPulse = 0;
  Serial.println("New pulse");
}

void pulseEnd()
{

  wasLow = false;

  Serial.println("End pulse");
  Serial.print(thisPulse);

  Serial.println(" pulse ms");

  snprintf(msg, 50, "%ld", (int)pulsePeriod);

  if (thisPulse < 80)
  {
    if (isCounting)
    {
      queueClient.sendQueue("pulsePeriod", msg);
      Serial.println("Pulse OK");
    }
    else
    {
      Serial.println("Pulse period counting started");
    }

    checkpoint();
  }
  else
  {
    log("Pulse Too Long");
    log(msg);
  }

  isCounting = true;

  pulsePeriod = 0;
}

void checkpoint()
{
  impressionsCounted++;

  Serial.print("Counting:  checkpoint session");
  Serial.print(impressionsCounted);
  Serial.println();
}



void log(char *message)
{
  Serial.println(message);
  queueClient.sendQueue("log", message);
}
