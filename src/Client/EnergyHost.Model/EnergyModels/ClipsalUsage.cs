using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyHost.Model.EnergyModels
{
    public class ClipsalUsage
    {
        public string datetime { get; set; }
        public double? pv_site_net__1 { get; set; }
        public double? consumed { get; set; }
        public double? load_oven__1 { get; set; }
        public double? load_powerpoint__1 { get; set; }
        public double? load_residual { get; set; }
        public double? ac_load_net { get; set; }
        public double? sold { get; set; }
        public double? saved { get; set; }
        public double? battery_storage__1 { get; set; }
        public double? bought { get; set; }
        public double? load_air_conditioner__1 { get; set; }
        public int? clipsal_solar_id { get; set; }
        public double? total_consumed { get; set; }
    }

    public class ClipsalInflux
    {
        public DateTime date { get; set; }
        public double powerpoints { get; set; }
        public double oven { get; set; }
        public double ac { get; set; }
        public double other { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Appliance
    {
        public string assignment { get; set; }
        public double power { get; set; }
        public string display_name { get; set; }
    }

    public class Weather
    {
        public string datetime { get; set; }
        public string condition { get; set; }
        public string description { get; set; }
        public double temperature { get; set; }
        public string icon { get; set; }
        public int id { get; set; }
        public bool daytime { get; set; }
    }

    public class ClipsalInstant
    {
        public double solar { get; set; }
        public double battery { get; set; }
        public string last_updated { get; set; }
        public List<Appliance> appliances { get; set; }
        public object battery_soc_fraction { get; set; }
        public object battery_duration_remaining { get; set; }
        public double grid { get; set; }
        public double consumption { get; set; }
        public double self_powered_fraction { get; set; }
        public bool blackout { get; set; }
        public double tariff_rate { get; set; }
        public Weather weather { get; set; }
    }




}
