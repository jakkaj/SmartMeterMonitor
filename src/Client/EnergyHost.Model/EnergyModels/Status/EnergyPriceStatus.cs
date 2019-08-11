using System;
using System.Collections.Generic;
using System.Text;
using EnergyHost.Model.EnergyModels.Status.Base;

namespace EnergyHost.Model.EnergyModels.Status
{
    public class EnergyPriceStatus:StatusBase
    {
        public double CurrentPriceIn { get; set; }
        public double CurrentPriceOut { get; set; }
    }
}
