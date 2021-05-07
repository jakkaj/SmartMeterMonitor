using System;
using System.Collections.Generic;
using System.Text;
using EnergyHost.Model.EnergyModels;

namespace EnergyHost.Model.DataModels
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Meter
    {
        public string last_communication_time { get; set; }
        public double instant_power { get; set; }
        public double instant_reactive_power { get; set; }
        public double instant_apparent_power { get; set; }
        public double frequency { get; set; }
        public double energy_exported { get; set; }
        public double energy_imported { get; set; }
        public double instant_average_voltage { get; set; }
        public double instant_total_current { get; set; }
        public double i_a_current { get; set; }
        public double i_b_current { get; set; }
        public double i_c_current { get; set; }
        public double timeout { get; set; }
    }

    
   

    public class Powerwall
    {
        
        public Meter solar { get; set; }
        public Meter site { get; set; }
        public Meter battery { get; set; }
        public Meter load { get; set; }
        public double charge { get; set; }
    }


}
