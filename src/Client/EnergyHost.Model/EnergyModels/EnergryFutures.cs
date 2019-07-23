using System;
using System.Collections.Generic;
using System.Text;
using DarkSky.Models;

namespace EnergyHost.Model.EnergyModels
{
    public class EnergryFutures
    {
        public List<EnergyFuture> Futures { get; set; }
    }

    public class EnergyFuture
    {
        public double Value { get; set; }
        public double SolarValue { get; set; }
        public double GridValue { get; set; }
        public DateTime Period { get; set; }
        public double SolarBad { get; set; }
        public double Cloudiness { get; set; }
        public double PriceIn { get; set; }
        public double PriceInNormalised { get; set; }
        public double PriceOut { get; set; }
        public double PriceOutNormalised { get; set; }
        public DataPoint DarkSkyDataPoint { get; set; }
        public VariablePricesAndRenewable AmberPrices { get; set; }
        public bool IsForecast { get; set; }
        public double UsageSuggestion { get; set; } //higher is better
        public double CostSuggestion { get; set; } //higher is better

    }
}
