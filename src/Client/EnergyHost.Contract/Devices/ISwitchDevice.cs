using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnergyHost.Contract.Devices
{
    public interface ISwitchDevice : IDevice
    {
        bool IsOn { get; set; }
        Task SwitchOff();
        Task SwitchOn();
    }
}
