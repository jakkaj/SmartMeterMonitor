using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyHost.Model.Settings
{
    public class EnergyHostSettings
    {
        public bool SuppressWarning { get; set; }
        public string AMBER_API_URL { get; set; }
        public string DARK_SKY_API_KEY { get; set; }
        public string PostCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
