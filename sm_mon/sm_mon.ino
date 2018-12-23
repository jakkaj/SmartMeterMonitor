#include <elapsedMillis.h>
#include "Statistic.h"

#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>

#include <PubSubClient.h>
//const char* ssid = "CMO2";
//const char* password = "HellaAwesome2";


const char *ssid = "TelstraAE6511";
const char *password = "bpjzhvssks";
const char* mqtt_server = "10.0.0.59";

const boolean wifiAlwaysOn = true;
const boolean everyPulse = true;

WiFiClient espClient;
PubSubClient client(espClient);

elapsedMillis timeElapsed;
elapsedMillis thisPulse;
elapsedMillis pulsePeriod;


Statistic stats;

int impressionsCounted = 0;
int lastPeriod = 0;
int resetCounter = 0;

bool wasLow = false;

bool isCounting = false;

void MQTT_connect();

void setup()
{

  Serial.begin(115200);
  wifiOff();
  delay(1);
  
  post("http://10.0.0.38:5000/impress/boot");
  client.setServer(mqtt_server, 1883);
  sendQueue("log", "Booted");
}




void loop()
{
  delay(10);
  // put your main code here, to run repeatedly:
  int val = analogRead(0);
  if(!wasLow){
    stats.add(val);
  }
  
  if(stats.count() < 100){
    return;
  }
  
  float avg = stats.average();

  //when we don't know the blink level yet
  //blink event will lower the val by some margin we don't yet know
  boolean isLow = false;

  float avgPercent = avg * 20 / 100;
  
  if (val < avg - avgPercent)
  {
    //Serial.println(val);
    //Serial.println("VAL low");    
    isLow = true;    
  }

  if(isLow && !wasLow){
    //this is a new pulse
    pulseStart();    
  }else if(isLow && wasLow){
    //still low
    wasLow = true;        
  }else if(!isLow && wasLow){
    //a finishing pulse   
    pulseEnd();
    delay(400); //we can delay a bit to save power as there is no chance of action just now
  }else{
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
  post(sendUrl);
  lastPeriod = impressionsCounted;
  impressionsCounted = 0;
  isCounting = false;
  Serial.println("Sending done");
}

void pulseStart(){
  if(pulsePeriod > 0){
    Serial.println("Time between");
    Serial.println(pulsePeriod);
  }

  wasLow = true;    
  thisPulse = 0;
  Serial.println("New pulse");  
}

char msg[50];

void pulseEnd(){
  

  wasLow = false;
    
  Serial.println("End pulse");
  Serial.print (thisPulse);
  
  Serial.println(" pulse ms");
  if(thisPulse < 80){
    
    snprintf (msg, 50, "%ld", (int)pulsePeriod);
    
    sendQueue("pulsePeriod", msg);
    
    Serial.println("Pulse OK");
    checkpoint();    
  }else{
    Serial.println("Pulse Too Long");
  }

  pulsePeriod = 0;
}

void sendQueue(char* topic, char* msg){

   Serial.println("Sending: ");
   Serial.println(msg);
   
   if (!client.connected()) {
      reconnectQueue();
   }
   
   client.publish(topic, msg);
}

void checkpoint()
{  
  impressionsCounted++;
  
  Serial.print("Counting:  checkpoint session");
  Serial.print(impressionsCounted);
  Serial.println();
  
}


void wifiOff()
{
  if(!wifiAlwaysOn){
    WiFi.mode(WIFI_OFF);
    WiFi.forceSleepBegin();
  }
}
void post(String sendUrl)
{
  Serial.println(sendUrl);

  //static ip
  IPAddress ip(10, 0, 0, 190);
  IPAddress gateway(10, 0, 0, 138);
  IPAddress subnet(255, 255, 255, 0);

  if(!wifiAlwaysOn){
    WiFi.forceSleepWake();
  }
  
  delay(1);

  WiFi.persistent(false);

  // Bring up the WiFi connection
  if(!wifiAlwaysOn){
    WiFi.mode(WIFI_STA);  
  }
  
  WiFi.config(ip, gateway, subnet);

  if(WiFi.status() != WL_CONNECTED){
      WiFi.begin(ssid, password);
  }
  

  while (WiFi.status() != WL_CONNECTED)
  {
    Serial.print(".");
    delay(1000);
  }

  Serial.print(WiFi.localIP());
  Serial.println(" Connected");

  if (WiFi.status() == WL_CONNECTED)
  { //Check WiFi connection status
    Serial.println("Sending");
    HTTPClient http; //Declare an object of class HTTPClient

    http.begin(sendUrl);       //Specify request destination
    int httpCode = http.GET(); //Send the request

    if (httpCode > 0)
    { //Check the returning code

      String payload = http.getString(); //Get the request response payload
      Serial.println(payload);           //Print the response payload
    }
    else
    {
      Serial.println("no response");
    }

    http.end(); //Close connection
  }
  else
  {
    Serial.println("Not connected");
  }

  if(!wifiAlwaysOn){
    WiFi.disconnect();
    WiFi.mode(WIFI_OFF);
    WiFi.forceSleepBegin();  
  }
  delay(1);
}



void reconnectQueue() {
  // Loop until we're reconnected
  while (!client.connected()) {
    Serial.print("Attempting MQTT connection...");
    // Create a random client ID
    String clientId = "ESP8266Client-";
    clientId += String(random(0xffff), HEX);
    // Attempt to connect
    if (client.connect(clientId.c_str())) {
      Serial.println("connected");
      // Once connected, publish an announcement...
      client.publish("outTopic", "hello world");
      
    } else {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" try again in 5 seconds");
      // Wait 5 seconds before retrying
      delay(5000);
    }
  }
}
