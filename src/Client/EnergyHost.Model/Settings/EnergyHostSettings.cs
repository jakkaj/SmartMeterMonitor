﻿using System;
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

        public string NETATMO_CLIENT_ID { get; set; }
        public string NETATMO_CLIENT_SECRET { get; set; }
        public string NETATMO_USER_NAME { get; set; }
        public string NETATMO_PASSWORD { get; set; }
        public string NETATMO_DEVICE_ID { get; set; }

        public string TESLA_IP { get; set; }
        public string TESLA_RESERVE_URL { get; set; }
        public int TESLA_OVERNIGHT_RESERVE { get; set; }

        public string AMBER_DATA_URL { get; set; }

        public string CLIPSAL_DATA_URL { get; set; }
        public string CLIPSAL_INST_URL { get; set; }
        public string CLIPSAL_OVEN_URL { get; set; }
    }
}
