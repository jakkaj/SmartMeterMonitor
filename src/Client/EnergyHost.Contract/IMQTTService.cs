using System;
using System.Threading.Tasks;
using EnergyHost.Model.EnergyModels.Status.Base;

namespace EnergyHost.Contract
{
    public interface IMQTTService
    {
        event EventHandler<StatusEventArgs> EventReceived;
        double KWH { get; set; }
        Task Setup();
        Task Send(string topic = "events", string payload = null);
    }
}