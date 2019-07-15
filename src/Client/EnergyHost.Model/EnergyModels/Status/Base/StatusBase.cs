using System;
using Newtonsoft.Json;

namespace EnergyHost.Model.EnergyModels.Status.Base
{
    public class StatusBase
    {
        public string Name { get; set; }

        public Guid StatusId { get; } = Guid.NewGuid();
    }
}
