#include "HttpClient.h"

void HttpClient::init(){

  IPAddress ip(192, 168, 0, 209);
  IPAddress gateway(192, 168, 0, 1);
  IPAddress subnet(255, 255, 255, 0);
  WiFi.config(ip, gateway, subnet);
  WiFi.disconnect();
  WiFi.mode(WIFI_OFF);
}

void HttpClient::wifiOff()
{
  if (!_wifiAlwaysOn)
  {
    Serial.println("Wifi sleep");
    WiFi.disconnect();
    WiFi.mode(WIFI_OFF);
    delay(100);
    Serial.println("WIFI Status off");
    Serial.println(WiFi.status());   
  }
}

void HttpClient::wifiOn()
{
  
  if (!_wifiAlwaysOn || WiFi.status() != WL_CONNECTED)
  {   
    WiFi.mode(WIFI_STA);
    delay(1);
    WiFi.disconnect();    
  }

 
  if (WiFi.status() != WL_CONNECTED)
  {
    WiFi.begin(_ssid, _pwd);
    delay(1);
    Serial.println("Begin wifi");
  }
 
  while (WiFi.status() != WL_CONNECTED)
  {
    Serial.print(".");
    delay(500);
  }
  Serial.println("Connected");
}

void HttpClient::post(String sendUrl)
{

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
