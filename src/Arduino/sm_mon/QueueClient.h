
#ifndef QueueClient_h
#define QueueClient_h

#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <PubSubClient.h>

#include "HttpClient.h"

class QueueClient {
    public:
        QueueClient(HttpClient *httpClient, const char *mqtt_server){
            
            _client = PubSubClient(_espClient);     
            _client.setServer(mqtt_server, 1883);     

            _httpClient = httpClient;
        }
        void MQTT_connect();
        void sendQueue(char *topic, char *msg);
        void reconnectQueue();
        void setEnabled(boolean state);
        
    private: 
        WiFiClient _espClient;       
        PubSubClient _client;  
        HttpClient *_httpClient;     
        boolean _enabled;  
};

#endif
