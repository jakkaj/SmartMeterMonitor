using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnergyHost.Contract.Devices
{
    public interface IDevice
    {
        string Name { get; }
        string Type { get; }
    }
}
