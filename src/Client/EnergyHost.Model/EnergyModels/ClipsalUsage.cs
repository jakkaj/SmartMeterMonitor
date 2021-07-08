using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyHost.Model.EnergyModels
{
    public class ClipsalUsage
    {
        public string datetime { get; set; }
        public double pv_site_net__1 { get; set; }
        public double consumed { get; set; }
        public double load_oven__1 { get; set; }
        public double load_powerpoint__1 { get; set; }
        public double load_residual { get; set; }
        public double ac_load_net { get; set; }
        public double sold { get; set; }
        public double saved { get; set; }
        public double battery_storage__1 { get; set; }
        public double bought { get; set; }
        public double load_air_conditioner__1 { get; set; }
        public int clipsal_solar_id { get; set; }
        public double total_consumed { get; set; }
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
