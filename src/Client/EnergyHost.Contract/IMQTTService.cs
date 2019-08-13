using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EnergyHost.Model.EnergyModels.Status.Base;

namespace EnergyHost.Contract
{
    public interface IMQTTService
    {
        Dictionary<string,object> Values{get;set;}
        event EventHandler<StatusEventArgs> EventReceived;
        double KWH { get; set; }
        Task Setup();
        Task Send(string topic = "events", string payload = null);
        double GetDouble(string value);
    }
}