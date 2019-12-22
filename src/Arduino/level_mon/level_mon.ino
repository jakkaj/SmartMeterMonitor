/*
* Ultrasonic Sensor HC-SR04 and Arduino Tutorial
*
* by Dejan Nedelkovski,
* www.HowToMechatronics.com
*
*/

#include "HttpClient.h"
#include "QueueClient.h"
#include "secrets.h"

#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>

// defines pins numbers
const int trigPin = 4;
const int echoPin = 5;

char msg[50];

// defines variables
long duration;
int distance;


char *mqtt_server = "192.168.0.220";

const boolean wifiAlwaysOn = true;

HttpClient httpClient(SECRET_SSID, SECRET_PWD, wifiAlwaysOn);
QueueClient queueClient(&httpClient, mqtt_server);

void setup() {

  httpClient.init();

  delay(200);

  queueClient.setEnabled(true);
  
  pinMode(trigPin, OUTPUT); // Sets the trigPin as an Output
  pinMode(echoPin, INPUT); // Sets the echoPin as an Input
  Serial.begin(9600); // Starts the serial communication
}

void loop() {
  // Clears the trigPin
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);
  
  // Sets the trigPin on HIGH state for 10 micro seconds
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);
  
  // Reads the echoPin, returns the sound wave travel time in microseconds
  duration = pulseIn(echoPin, HIGH);
  
  // Calculating the distance
  distance= duration*0.034/2;
  
  // Prints the distance on the Serial Monitor
  Serial.print("Distance: ");
  Serial.println(distance);

  send(distance);
  
  delay(10000);
}


void send(int distance)
{ 
    snprintf(msg, 50, "%ld", distance);
    queueClient.sendQueue("tankheight", msg);
 
}
