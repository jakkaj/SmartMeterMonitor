#include "QueueClient.h"

void QueueClient::setEnabled(boolean state){
  _enabled = state;
}

void QueueClient::sendQueue(char *topic, char *msg)
{
   
    if(!_enabled){
      return;
    }   

    _httpClient->wifiOn();
    
    if (!_client.connected())
    {
        reconnectQueue();
    }

    _client.publish(topic, msg);

    _httpClient->wifiOff();
}

void QueueClient::reconnectQueue()
{
  // Loop until we're reconnected
  while (!_client.connected())
  {
    Serial.print("Attempting MQTT connection...");
    // Create a random client ID
    String clientId = "ESP8266Client-";
    clientId += String(random(0xffff), HEX);
    // Attempt to connect
    if (_client.connect(clientId.c_str()))
    {
      Serial.println("connected");
      // Once connected, publish an announcement...
      _client.publish("outTopic", "hello world");
    }
    else
    {
      Serial.print("failed, rc=");
      Serial.print(_client.state());
      Serial.println(" try again in 5 seconds");
      // Wait 5 seconds before retrying
      delay(5000);
    }
  }
}
