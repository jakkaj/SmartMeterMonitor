
#ifndef QueueClient_h
#define QueueClient_h

#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <PubSubClient.h>

class QueueClient {
    public:
        QueueClient(const char *mqtt_server){
            WiFiClient espClient;
            _client = PubSubClient(espClient);     
            _client.setServer(mqtt_server, 1883);       
        }
        void MQTT_connect();
        void sendQueue(char *topic, char *msg);
        void reconnectQueue();
        
    private:        
        PubSubClient _client;         
};

#endif
