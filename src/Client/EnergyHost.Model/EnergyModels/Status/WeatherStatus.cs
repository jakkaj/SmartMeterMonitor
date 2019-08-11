using System;
using System.Collections.Generic;
using System.Text;
using EnergyHost.Model.EnergyModels.Status.Base;

namespace EnergyHost.Model.EnergyModels.Status
{
    public class WeatherStatus : StatusBase
    {
        public double MinToday { get; set; }
        public double MaxToday { get; set; }
        public double MaxTomorrow { get; set; }
        public double MinTomorrow { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
        public double WindSpeed { get; set; }
        public double CurrentTemp { get; set; }
    }
}
