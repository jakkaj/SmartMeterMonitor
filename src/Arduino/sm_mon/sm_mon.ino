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
const char *mqtt_server = "10.0.0.220";

const boolean wifiAlwaysOn = true;

HttpClient httpClient(ssid, password, wifiAlwaysOn);
QueueClient queueClient(&httpClient, mqtt_server);

elapsedMillis timeElapsed;
elapsedMillis thisPulse;
elapsedMillis pulsePeriod;
elapsedMillis pulseSimulate;

Statistic stats;

int impressionsCounted = 0;
int lastPeriod = 0;
int resetCounter = 0;

char msg[50];

bool wasLow = false;
bool simulate = false;

bool isCounting = false;

void setup()
{
  
  queueClient.setEnabled(wifiAlwaysOn);

  Serial.begin(115200);  

  
  httpClient.wifiOff();
  delay(1);

  httpClient.post("http://10.0.0.220:5000/impress/boot");
  
  
  if(simulate){
    log("****Simulate - booted");
  }else{
    log("Booted");
  }
}

void loop()
{
  delay(10);
  // put your main code here, to run repeatedly:
  int val = analogRead(0);
  //Serial.println(val);
  if (!wasLow && stats.count() < 1000)
  {
    stats.add(val);
  }

  if (stats.count() < 1000)
  {
    if(stats.count() % 10 == 0){
      log("Collecting stats");
    }
    return;
  }else if(stats.count() <= 1000){
    stats.add(val);
    log("variance", stats.variance());
    log("minimum", stats.minimum());
    log("maximum", stats.maximum());
    log("pop_stdev", stats.pop_stdev());
    log("unbiased_stdev", stats.unbiased_stdev());
    log("average", stats.average());
    float lowCutoff2 = (stats.maximum() - stats.minimum())/2;
    log("cutoff", lowCutoff2);
  }
  
  float avg = stats.average();
  float stddev = stats.pop_stdev();
  float variance = stats.variance();
  float minimum=stats.minimum();
  float maximum=stats.maximum();

  float lowCutoff = (maximum - minimum)/2;
  //when we don't know the blink level yet
  //blink event will lower the val by some margin we don't yet know
  boolean isLow = false;

  float tooLow = avg - lowCutoff;

 // float avgPercent = avg * 20 / 100;
 // float avgPercentTen = avg * 10 / 100;

  //if (val < avg - avgPercentTen)
  //{
    //log("ten percent");
    //log(val);
    
  //}

  if (val < tooLow)
  {
    isLow = true;
  }

  if (isLow && !wasLow)
  {
    log("start with val", val);
    log("Current average", avg);
    log("Current stddev", stddev);
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
    log("end with val", val);
    //a finishing pulse
    pulseEnd();
    delay(400); //we can delay a bit to save power as there is no chance of action just now
  }
  else
  {
    //nothing
  }

  if(simulate){
    if(pulseSimulate / 1000 > 5){
      pulseSimulate = 0;
      pulseStart();
      delay(30);
      pulseEnd();
    }
  }

  if (timeElapsed / 1000 > 600)
  {
    stats.clear();
    timeElapsed = 0;
    send();
    isLow = false;
    wasLow = false;
  }
}

void send()
{
  //in wifialways mode the mqtt does this work for every pulse (needs the mqtt client .netapp running)
  
  isCounting = false;
  
  if(wifiAlwaysOn){
    log("****************    HeartBeat");
    Serial.println("Not sending to web, wifiAlwaysOn is set");
    return;
  }
  
  Serial.print("Sending impressions:");
  Serial.print(impressionsCounted);
  Serial.println();

  String url = "http://10.0.0.220:5000/impress?time=600&imp=";
  String sendUrl = url + impressionsCounted;
  httpClient.post(sendUrl);

  snprintf(msg, 50, "%ld", impressionsCounted);
  queueClient.sendQueue("impressions", msg);

  lastPeriod = impressionsCounted;

  impressionsCounted = 0;
  
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
  //Serial.println("New pulse");
}

void pulseEnd()
{

  wasLow = false;

  log("End pulse");
  log(thisPulse);

  log(" pulse ms");

  snprintf(msg, 50, "%ld", (int)pulsePeriod);

  if (thisPulse < 80)
  {
    if (isCounting)
    {
      queueClient.sendQueue("pulsePeriod", msg);
      //Serial.println("Pulse OK");
    }
    else
    {
      log("Pulse period counting started");
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

void log(char *message, float value)
{
  Serial.println(message);
  queueClient.sendQueue("log", message);
  Serial.println(value);
  snprintf(msg, 50, "%f", value);
  queueClient.sendQueue("log", msg);
}


void log(char *message)
{
  Serial.println(message);
  queueClient.sendQueue("log", message);
}