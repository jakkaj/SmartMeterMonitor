using System;
using System.Collections.Generic;
using System.Text;
using EnergyHost.Model.EnergyModels.Status.Base;

namespace EnergyHost.Model.EnergyModels.Status
{
    public class DaikinStatus : StatusBase
    {
        public double DaikinSetTemperature { get; set; }
        public double DaikinInsideTemperature { get; set; }
        public string DaikinMode { get; set; }
        public bool DaikinPoweredOn { get; set; }
    }
}
