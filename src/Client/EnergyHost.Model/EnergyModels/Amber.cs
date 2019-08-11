using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EnergyHost.Model.EnergyModels
{
    public class Price
    {
        public bool dataAvailable { get; set; }
        public double networkDailyPrice { get; set; }
        public double basicMeterDailyPrice { get; set; }
        public double additionalSmartMeterDailyPrice { get; set; }
        public double amberDailyPrice { get; set; }
        public double totalDailyPrice { get; set; }
        public double networkKWHPrice { get; set; }
        public double marketKWHPrice { get; set; }
        public double greenKWHPrice { get; set; }
        public double carbonNeutralKWHPrice { get; set; }
        public double lossFactor { get; set; }
        public double offsetKWHPrice { get; set; }
        public double totalfixedKWHPrice { get; set; }
        public double totalBlackPeakFixedKWHPrice { get; set; }
        public double totalBlackShoulderFixedKWHPrice { get; set; }
        public double totalBlackOffpeakFixedKWHPrice { get; set; }
    }

    public class StaticPrices
    {
        public Price E1 { get; set; }
        public Price E2 { get; set; }
        public Price B1 { get; set; }
        public Price B1PFIT { get; set; }
    }

    public class VariablePricesAndRenewable
    {
        public DateTime createdAt { get; set; }
        public string periodType { get; set; }
        public double wholesaleKWHPrice { get; set; }
        public string region { get; set; }
        public DateTime period { get; set; }
        public double renewablesPercentage { get; set; }
        public string percentileRank { get; set; }
        public DateTime? forecastedAt { get; set; }
        [JsonProperty("forecastedAt+period")]
        public string forecastedAtplusperiod { get; set; }
        public double InPrice { get; set; }
        public double InPriceNormal { get; set; }
        public double OutPrice { get; set; }
        public double OutPriceNormal { get; set; }
    }

    public class Data
    {
        public DateTime currentNEMtime { get; set; }
        public string postcode { get; set; }
        public string networkProvider { get; set; }
        public StaticPrices staticPrices { get; set; }
        public List<VariablePricesAndRenewable> variablePricesAndRenewables { get; set; }
    }

    public class AmberData
    {
        public int serviceResponseType { get; set; }
        public Data data { get; set; }
        public string message { get; set; }
    }
}
