using System;
using Newtonsoft.Json;

namespace EnergyHost.Model.EnergyModels.Status.Base
{
    public class StatusBase
    {
        public string Name { get; }

        public StatusBase(string name)
        {
            this.Name = name;
        }
        public bool IsReady { get; set; }

        [JsonIgnore]
        public EventHandler StatusUpdated;

        public void UpdateStatus(object sender)
        {
            StatusUpdated?.Invoke(sender, EventArgs.Empty);
        }

    }

    
}
