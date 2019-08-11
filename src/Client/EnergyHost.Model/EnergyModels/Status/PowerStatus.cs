using System;
using System.Collections.Generic;
using System.Text;
using EnergyHost.Model.EnergyModels.Status.Base;

namespace EnergyHost.Model.EnergyModels.Status
{
    public class PowerStatus : StatusBase
    {
        public double KWHIn { get; set; }
        public double KWHOut { get; set; }
        public double KWHUsed { get; set; }
        public double KWHSolar { get; set; }
        public double KWHSolarToday { get; set; }
    }
}
