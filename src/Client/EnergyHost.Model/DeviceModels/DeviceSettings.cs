using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyHost.Model.DeviceModels
{
    public class DeviceSettings
    {
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public Dictionary<string,string> Settings { get; set; }

    }
}
