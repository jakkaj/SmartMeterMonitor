#include "HttpClient.h"

void HttpClient::wifiOff()
{
  if (!_wifiAlwaysOn)
  {
    WiFi.disconnect(true);
    WiFi.mode(WIFI_OFF);
    WiFi.forceSleepBegin();
    delay(1);
    //Serial.println("Wifi off");    
    
  }
}

void HttpClient::wifiOn(){
  //static ip
  IPAddress ip(10, 0, 0, 190);
  IPAddress gateway(10, 0, 0, 138);
  IPAddress subnet(255, 255, 255, 0);

  if (!_wifiAlwaysOn)
  {
    WiFi.forceSleepWake();
    delay(1);
  }
  
  // Bring up the WiFi connection
  if (!_wifiAlwaysOn)
  {
    WiFi.mode(WIFI_STA);
  }

  WiFi.config(ip, gateway, subnet);

  if(WiFi.status() != WL_CONNECTED || !_wifiAlwaysOn){
    WiFi.begin(_ssid, _pwd);
  }    
  
  while (WiFi.status() != WL_CONNECTED)
  {
    Serial.print(".");
    delay(1000);
  }  
}

void HttpClient::post(String sendUrl){
    Serial.println(sendUrl);

  wifiOn();

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
  
  wifiOff();
  
  delay(1);
}
