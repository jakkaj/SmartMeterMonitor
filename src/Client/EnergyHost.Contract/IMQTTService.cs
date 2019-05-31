using System;
using System.Threading.Tasks;

namespace EnergyHost.Contract
{
    public interface IMQTTService
    {
        event EventHandler MessageReceived;
        double KWH { get; set; }
        Task Setup();
    }
}