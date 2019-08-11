#ifndef HttpClient_h
#define HttpClient_h

#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>

class HttpClient {
    public:
        HttpClient(const char* ssid, const char* pwd, const boolean wifiAlwaysOn){
            _wifiAlwaysOn = wifiAlwaysOn;
            _ssid = ssid;
            _pwd = pwd;
        }

        void post(String sendUrl);
        void wifiOff();
        void wifiOn();
        void init();
  
    private:
        boolean _wifiAlwaysOn;
        const char * _ssid;
        const char * _pwd;
};

#endif
