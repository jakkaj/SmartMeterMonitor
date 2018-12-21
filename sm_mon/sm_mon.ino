#include <elapsedMillis.h>
#include "Statistic.h" 

#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>

//const char* ssid = "CMO2";
//const char* password = "HellaAwesome2";

const char* ssid = "TelstraAE6511";
const char* password = "bpjzhvssks";


elapsedMillis timeElapsed;
elapsedMillis lastPulse;
Statistic stats;


int impressionsCounted = 0;

bool isCounting = false;

void setup() {
  
  Serial.begin(115200);
  post("http://10.0.0.38:5000/impress/boot");

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
  
  
  
  if(timeElapsed / 1000 > 15){ 

    String debugStart = "Avg";
    debugStart = debugStart + avg;
    debugStart += "val";
    debugStart += val;
      

    String url = "http://10.0.0.38:5000/impress/debug?debugString=";
    String sendUrl = url + debugStart;
    post(sendUrl);
    stats.clear();
    timeElapsed = 0;
    send();
  }
  
}

void post(String sendUrl){
   Serial.println(sendUrl);

  if (WiFi.status() != WL_CONNECTED) {    
    WiFi.begin(ssid, password);
  }

  while (WiFi.status() != WL_CONNECTED) {
    
    Serial.print(".");
    delay(1000);   
  }
  
  Serial.print(WiFi.localIP());
  Serial.println(" Connected");
  
  
  if (WiFi.status() == WL_CONNECTED) { //Check WiFi connection status
    Serial.println("Sending");
    HTTPClient http;  //Declare an object of class HTTPClient

    
   
    http.begin(sendUrl);  //Specify request destination
    int httpCode = http.GET();                                                                  //Send the request
 
    if (httpCode > 0) { //Check the returning code
 
      String payload = http.getString();   //Get the request response payload
      Serial.println(payload);                     //Print the response payload
 
    }else{
      Serial.println("no response");
    }
 
    http.end();   //Close connection
 
  }else{
    Serial.println("Not connected");
  }

  WiFi.disconnect();
}

void send(){
  Serial.print("Sending impressions:");
  Serial.print(impressionsCounted);
  Serial.println();

  String url = "http://10.0.0.38:5000/impress?time=15&imp=";
  String sendUrl = url + impressionsCounted;
  post(sendUrl);
  impressionsCounted = 0;
  isCounting = false;
  Serial.println("Sending done");
  
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

