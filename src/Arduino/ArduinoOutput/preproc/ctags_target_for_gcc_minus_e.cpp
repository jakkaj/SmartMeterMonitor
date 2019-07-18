# 1 "/Users/jak/GitHub/SmartMeterMonitor/src/Arduino/sm_mon/sm_mon.ino"
# 1 "/Users/jak/GitHub/SmartMeterMonitor/src/Arduino/sm_mon/sm_mon.ino"
# 2 "/Users/jak/GitHub/SmartMeterMonitor/src/Arduino/sm_mon/sm_mon.ino" 2
# 3 "/Users/jak/GitHub/SmartMeterMonitor/src/Arduino/sm_mon/sm_mon.ino" 2

# 5 "/Users/jak/GitHub/SmartMeterMonitor/src/Arduino/sm_mon/sm_mon.ino" 2
# 6 "/Users/jak/GitHub/SmartMeterMonitor/src/Arduino/sm_mon/sm_mon.ino" 2

# 8 "/Users/jak/GitHub/SmartMeterMonitor/src/Arduino/sm_mon/sm_mon.ino" 2
# 9 "/Users/jak/GitHub/SmartMeterMonitor/src/Arduino/sm_mon/sm_mon.ino" 2

# 11 "/Users/jak/GitHub/SmartMeterMonitor/src/Arduino/sm_mon/sm_mon.ino" 2
//const char* ssid = "CMO2";
//const char* password = "HellaAwesome2";

const char *ssid = "TelstraCAC61A";
const char *password = "mm2mdkbfrn";
const char *mqtt_server = "192.168.0.220";

const boolean wifiAlwaysOn = true;

HttpClient httpClient(ssid, password, wifiAlwaysOn);
QueueClient queueClient(&httpClient, mqtt_server);

elapsedMillis timeElapsed;
elapsedMillis thisPulse;
elapsedMillis pulsePeriod;
elapsedMillis pulseSimulate;
elapsedMillis actualPeriod;

Statistic stats;

int impressionsCounted = 0;
int lastPeriod = 0;
int resetCounter = 0;

char msg[50];

bool wasLow = false;
bool simulate = true;

bool isCounting = false;

bool pulseSinceLastSend = false;

void setup()
{

  Serial.begin(115200);

  httpClient.init();

  delay(200);

  queueClient.setEnabled(true);

  //httpClient.post("http://192.168.0.220:5000/impress/boot");

  if (simulate)
  {
    log("****Simulate - booted");
  }
  else
  {
    log("Booted");
  }
}

void loop()
{
  delay(10);
  // put your main code here, to run repeatedly:
  int val = analogRead(0);
  //Serial.println(val);
  if (!wasLow && stats.count() < 1000)
  {
    stats.add(val);
  }

  if (stats.count() < 1000)
  {
    if (stats.count() % 10 == 0)
    {
      log("Collecting stats");
    }
    return;
  }
  else if (stats.count() <= 1000)
  {
    stats.add(val);
    log("variance", stats.variance());
    log("minimum", stats.minimum());
    log("maximum", stats.maximum());
    log("pop_stdev", stats.pop_stdev());
    log("unbiased_stdev", stats.unbiased_stdev());
    log("average", stats.average());
    float lowCutoff2 = (stats.maximum() - stats.minimum()) / 2;
    log("cutoff", lowCutoff2);
  }

  float avg = stats.average();
  float stddev = stats.pop_stdev();
  float variance = stats.variance();
  float minimum = stats.minimum();
  float maximum = stats.maximum();

  float lowCutoff = (maximum - minimum) / 2;
  //when we don't know the blink level yet
  //blink event will lower the val by some margin we don't yet know
  boolean isLow = false;

  float tooLow = avg - lowCutoff;

  if (val < tooLow)
  {
    isLow = true;
  }

  if (isLow && !wasLow)
  {
    //log("start with val", val);
    //log("Current average", avg);
    //log("Current stddev", stddev);
    //this is a new pulse
    pulseStart();
  }
  else if (isLow && wasLow)
  {
    //still low
    wasLow = true;
  }
  else if (!isLow && wasLow)
  {
    log("end with val", val);
    //a finishing pulse
    pulseEnd();
    //delay(400); //we can delay a bit to save power as there is no chance of action just now
  }
  else
  {
    //nothing
  }

  if (simulate)
  {
    if (pulseSimulate / 1000 > 5)
    {
      log("Simluate");
      pulseSimulate = 0;
      pulseStart();
      delay(60);
      pulseEnd();
    }
  }

  //log("Jordan");
  if ((timeElapsed / 1000) % 10 == 0)
  { //send every 10 seconds

    send();
  }

  if (timeElapsed / 1000 > 1600)
  {
    log("reset");
    stats.clear();
    send();
    timeElapsed = 0;
    //
    isLow = false;
    wasLow = false;
  }
}

void send()
{
  if (pulseSinceLastSend)
  {
    snprintf(msg, 50, "%ld", (int)actualPeriod);

    queueClient.sendQueue("pulsePeriod", msg);
    pulseSinceLastSend = false;
  }
}

void pulseStart()
{


  wasLow = true;
  thisPulse = 0;
  //Serial.println("New pulse");
}

void pulseEnd()
{

  wasLow = false;

  snprintf(msg, 50, "%ld", (int)pulsePeriod);

  //log(thisPulse);

  if (thisPulse < 80)
  {
    actualPeriod = pulsePeriod;
    pulseSinceLastSend = true;
  }
  else
  {
    log("Pulse Too Long");
  }

  pulsePeriod = 0;

  isCounting = true;
}

void log(char *message, float value)
{
  Serial.println(message);

  Serial.println(value);
  snprintf(msg, 50, "%f", value);
}

void log(char *message)
{
  Serial.println(message);
  //queueClient.sendQueue("log", message);
}
