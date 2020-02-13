using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyHost.Model.EnergyModels
{
    public class B1
    {
        public double totalUsageInCertainPeriod { get; set; }
        public double totalUsageCostInCertainPeriod { get; set; }
        public double lessThanAverageCost { get; set; }
        public double lessThanAverageUsage { get; set; }
        public double savedCost { get; set; }
        public double usedPriceSpikesInCertainPeriod { get; set; }
        public double lessThanAveragePrice { get; set; }
    }

    public class E1
    {
        public double totalUsageInCertainPeriod { get; set; }
        public double totalUsageCostInCertainPeriod { get; set; }
        public double lessThanAverageCost { get; set; }
        public double lessThanAverageUsage { get; set; }
        public double savedCost { get; set; }
        public double usedPriceSpikesInCertainPeriod { get; set; }
        public double lessThanAveragePrice { get; set; }
    }

    public class ThisWeekUsage
    {
        public B1 B1 { get; set; }
        public E1 E1 { get; set; }
    }

    public class B12
    {
        public double totalUsageInCertainPeriod { get; set; }
        public double totalUsageCostInCertainPeriod { get; set; }
        public double lessThanAverageCost { get; set; }
        public double lessThanAverageUsage { get; set; }
        public double savedCost { get; set; }
        public double usedPriceSpikesInCertainPeriod { get; set; }
        public double lessThanAveragePrice { get; set; }
    }

    public class E12
    {
        public double totalUsageInCertainPeriod { get; set; }
        public double totalUsageCostInCertainPeriod { get; set; }
        public double lessThanAverageCost { get; set; }
        public double lessThanAverageUsage { get; set; }
        public double savedCost { get; set; }
        public double usedPriceSpikesInCertainPeriod { get; set; }
        public double lessThanAveragePrice { get; set; }
    }

    public class LastWeekUsage
    {
        public B12 B1 { get; set; }
        public E12 E1 { get; set; }
    }

    public class B13
    {
        public double totalUsageInCertainPeriod { get; set; }
        public double totalUsageCostInCertainPeriod { get; set; }
        public double lessThanAverageCost { get; set; }
        public double lessThanAverageUsage { get; set; }
        public double savedCost { get; set; }
        public double usedPriceSpikesInCertainPeriod { get; set; }
        public double lessThanAveragePrice { get; set; }
    }

    public class E13
    {
        public double totalUsageInCertainPeriod { get; set; }
        public double totalUsageCostInCertainPeriod { get; set; }
        public double lessThanAverageCost { get; set; }
        public double lessThanAverageUsage { get; set; }
        public double savedCost { get; set; }
        public double usedPriceSpikesInCertainPeriod { get; set; }
        public double lessThanAveragePrice { get; set; }
    }

    public class LastMonthUsage
    {
        public B13 B1 { get; set; }
        public E13 E1 { get; set; }
    }

    public class ThisWeekDailyUsage
    {
        public DateTime date { get; set; }
        public string usageType { get; set; }
        public double usageCost { get; set; }
        public double usageKWH { get; set; }
        public double usageAveragePrice { get; set; }
        public double usagePriceSpikes { get; set; }
        public double dailyFixedCost { get; set; }
        public string meterSuffix { get; set; }
    }

    public class LastWeekDailyUsage
    {
        public DateTime date { get; set; }
        public string usageType { get; set; }
        public double usageCost { get; set; }
        public double usageKWH { get; set; }
        public double usageAveragePrice { get; set; }
        public double usagePriceSpikes { get; set; }
        public double dailyFixedCost { get; set; }
        public string meterSuffix { get; set; }
    }

    public class LastMonthDailyUsage
    {
        public DateTime date { get; set; }
        public string usageType { get; set; }
        public double usageCost { get; set; }
        public double usageKWH { get; set; }
        public double usageAveragePrice { get; set; }
        public double usagePriceSpikes { get; set; }
        public double dailyFixedCost { get; set; }
        public string meterSuffix { get; set; }
    }

    public class PriceData
    {
        public DateTime currentNEMtime { get; set; }
        public ThisWeekUsage thisWeekUsage { get; set; }
        public LastWeekUsage lastWeekUsage { get; set; }
        public LastMonthUsage lastMonthUsage { get; set; }
        public List<ThisWeekDailyUsage> thisWeekDailyUsage { get; set; }
        public List<LastWeekDailyUsage> lastWeekDailyUsage { get; set; }
        public List<LastMonthDailyUsage> lastMonthDailyUsage { get; set; }
        public List<string> usageDataTypes { get; set; }
    }

    public class AmberUsage
    {
        public PriceData data { get; set; }
        public int serviceResponseType { get; set; }
        public object message { get; set; }
    }
}
