#include <elapsedMillis.h>
#include "Statistic.h" 

#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>

const char* ssid = "TelstraAE6511";
const char* password = "bpjzhvssks";

elapsedMillis timeElapsed;
elapsedMillis lastPulse;
Statistic stats;


int impressionsCounted = 0;

bool isCounting = false;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);

  WiFi.begin(ssid, password);
 
  while (WiFi.status() != WL_CONNECTED) {
 
    delay(1000);
    Serial.println("Connecting..");
 
  }
  Serial.println("Connected");
}

void loop() {
  delay(10);
  // put your main code here, to run repeatedly:
  int val = analogRead(0);
  stats.add(val);
 
  float avg = stats.average();  
  
  if(val < avg - (100)){
    Serial.println(avg);
    Serial.println(val);
    //Serial.println(stdDev);
    Serial.println();
    checkpoint();
  }
  
  
  
  if(timeElapsed / 1000 > 30){  
    stats.clear();
    timeElapsed = 0;
    send();
  }
  
}

void send(){
  Serial.print("Sending impressions:");
  Serial.print(impressionsCounted);
  Serial.println();

  if(WiFi.status() != WL_CONNECTED){
    WiFi.begin(ssid, password);
  }

  while (WiFi.status() != WL_CONNECTED) {
 
    delay(1000);
    Serial.println("Connecting..");
 
  }
  
  Serial.println("Connected");
  
  if (WiFi.status() == WL_CONNECTED) { //Check WiFi connection status
 
    HTTPClient http;  //Declare an object of class HTTPClient
 
    http.begin("http://10.0.0.38:5000/impress?time=30&imp=" + impressionsCounted);  //Specify request destination
    int httpCode = http.GET();                                                                  //Send the request
 
    if (httpCode > 0) { //Check the returning code
 
      String payload = http.getString();   //Get the request response payload
      Serial.println(payload);                     //Print the response payload
 
    }
 
    http.end();   //Close connection
 
  }else{
    Serial.println("Not connected");
  }
  impressionsCounted = 0;
  isCounting = false;
  
} 

void checkpoint(){

  //only start counting when an impression occurrs
  if(!isCounting){   
    Serial.println("Beginning checkpoint session"); 
    isCounting = true;
    timeElapsed = 0;    
    impressionsCounted = 0;
    return;    
  }

  if(lastPulse < 500){
    return;
  }
  
  Serial.print("Counting:  checkpoint session"); 
  Serial.print(impressionsCounted);
  Serial.println(); 
  impressionsCounted ++; 
  lastPulse = 0; 
}

