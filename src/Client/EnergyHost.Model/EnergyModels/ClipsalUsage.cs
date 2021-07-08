using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyHost.Model.EnergyModels
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Component
    {
        public string type { get; set; }
        public double amount { get; set; }
        public string aggregation_type { get; set; }
    }

    public class Assignment
    {
        public string assignment { get; set; }
        public double amount { get; set; }
    }

    public class ClipsalUsage
    {
        public string date { get; set; }
        public List<Component> components { get; set; }
        public List<Assignment> assignments { get; set; }
        public double total_cost { get; set; }
    }

    public class ClipsalInflux
    {
        public DateTime date { get; set; }
        public double powerpoints { get; set; }
        public double oven { get; set; }
        public double ac { get; set; }
        public double other { get; set; }
    }


}
