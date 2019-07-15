using System;
using System.Collections.Generic;
using System.Text;
using EnergyHost.Model.EnergyModels.Status.Base;

namespace EnergyHost.Model.EnergyModels.Status
{
    public class TimeStatus : StatusBase
    {
        public DateTime CurrentDateTime { get; set; } = DateTime.UtcNow;

        public int CurrentMinute { get; set; } = DateTime.UtcNow.Minute;

        public int CurrentHour { get; set; } = DateTime.UtcNow.Hour;
    }
}
