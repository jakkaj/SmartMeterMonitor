using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyHost.Model.EnergyModels.Status.Base
{
    public class StatusEventArgs : EventArgs
    {
        public string Data { get; set; }
    }

    public class StatusUpdatedEventArgs : EventArgs
    {
        public string StatusName { get; set; }
    }
}
