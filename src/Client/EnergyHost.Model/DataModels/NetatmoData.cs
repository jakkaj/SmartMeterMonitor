using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyHost.Model.DataModels
{
    public class NetatmoData
    {
        public double? IndoorTemp { get; set; }
        public double? OutdoorTemp { get; set; }
        public double? AbsPressure { get; set; }
        public double? Pressure { get; set; }
        public double? CO2 { get; set; }
        public double? IndoorHumidity { get; set; }
        public double? OutdoorHumidity { get; set; }
        public double? Noise { get; set; }
        public double? Rain { get; set; }
        public double? Rain24 { get; set; }
        public int? WindAngle { get; set; }
        public int? WindStrength { get; set; }
        public int? WindGusts { get; set; }

    }
}
