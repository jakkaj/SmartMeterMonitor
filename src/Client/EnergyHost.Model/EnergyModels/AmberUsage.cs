using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace EnergyHost.Model.EnergyModels
{
    public class TotalUsage
    {
        public double totalUsageInCertainPeriod { get; set; }
        public double totalUsageCostInCertainPeriod { get; set; }
        public double lessThanAverageCost { get; set; }
        public double lessThanAverageUsage { get; set; }
        public double savedCost { get; set; }
        public double usedPriceSpikesInCertainPeriod { get; set; }
        public double lessThanAveragePrice { get; set; }
        public DateTime date { get; set; }
    }
    
    public class Usage
    {
        [JsonProperty("E1")]
        public TotalUsage FromGrid { get; set; }
        [JsonProperty("B1")]
        public TotalUsage ToGrid { get; set; }
    }

    public class DailyUsage
    {
        public DateTime date { get; set; }
        public string usageType { get; set; }
        public double usageCost { get; set; }
        public double usageKWH { get; set; }
        public double usageAveragePrice { get; set; }
        public double usagePriceSpikes { get; set; }
        public double dailyFixedCost { get; set; }
        public string meterSuffix { get; set; }
        public double actualCost { get; set; }
    }

    public class PriceData
    {
        public DateTime currentNEMtime { get; set; }
        public Usage thisWeekUsage { get; set; }
        public Usage lastWeekUsage { get; set; }
        public Usage lastMonthUsage { get; set; }
        public List<DailyUsage> thisWeekDailyUsage { get; set; }
        public List<DailyUsage> lastWeekDailyUsage { get; set; }
        public List<DailyUsage> lastMonthDailyUsage { get; set; }
        public List<string> usageDataTypes { get; set; }
    }

    public class AmberUsage
    {
        public PriceData data { get; set; }
        public int serviceResponseType { get; set; }
        public object message { get; set; }
    }
}
