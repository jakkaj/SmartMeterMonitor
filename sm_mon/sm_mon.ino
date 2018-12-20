#include <elapsedMillis.h>
#include "Statistic.h" 
elapsedMillis timeElapsed;
Statistic stats;

int val = 0;
int total = 0;
int readings = 0;
int currentAverage = 0;
int reportedAverage = 0;
void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
}

void loop() {
  delay(10);
  // put your main code here, to run repeatedly:
  val = analogRead(0);
  stats.add(val);
  //Serial.print(" Val: ");
 // Serial.print(val);
  //Serial.print("  Average: ");
 // Serial.print(stats.average(), 4);
  
 // Serial.print("  Std deviation: ");

  //Serial.print(stats.pop_stdev(), 4);
 // Serial.println();
  //Serial.println(val);
  float avg = stats.average();
  float stdDev = stats.pop_stdev();
  
  if(val < avg - (stdDev * 3)){
    Serial.println(avg);
    Serial.println(val);
    Serial.println(stdDev);
    Serial.println();
    checkpoint();
  }
  
  if(timeElapsed / 1000 > 5){
    Serial.println("Clear");
    Serial.println(avg);
    Serial.println(val);
    Serial.println(stdDev);
    stats.clear();
    timeElapsed = 0;
  }
  
}

void checkpoint(){
  Serial.println(" ** Checkpoint **");
}

//find the average low light so we can tell if there is a high light event later.


