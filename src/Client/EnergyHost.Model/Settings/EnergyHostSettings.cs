using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyHost.Model.Settings
{
    public class EnergyHostSettings
    {
        public bool SuppressWarning { get; set; }
        public string AMBER_API_URL { get; set; }
        public string AMBER_USAGE_URL { get; set; }
        public string AMBER_USERNAME { get; set; }
        public string AMBER_PASSWORD { get; set; }
        public string AMBER_LOGIN_URL { get; set; }
        public string DARK_SKY_API_KEY { get; set; }
        public string MQTT_SERVER_ADDRESS { get; set; }
        public string INFLUX_SERVER_ADDRESS { get; set; }
        public string DAIKIN_AUTH { get; set; }
        public string DAIKIN_URL { get; set; }
        public string ABB_AUTH { get; set; }
        public string ABB_URL { get; set; }
        public string ABB_MODBUS_URL { get; set; }
        public string PostCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double DaikinThreshold { get; set; }
        
        public string ALEXA_NOTIFICATION_KEY { get; set; } //https://www.thomptronics.com/about/notify-me
        public string PUSHOVER_USER { get; set; } //https://pushover.net/api
        public string PUSHOVER_TOKEN { get; set; }
    }
}
