using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyHost.Model.DataModels
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class CurrentPeriod
    {
        public DateTime start { get; set; }
        public int kwhPriceInCents { get; set; }
        public object kwhPriceRange { get; set; }
        public int renewablePercentage { get; set; }
        public string indicator { get; set; }
        public string __typename { get; set; }
    }

    public class PreviousPeriod
    {
        public DateTime start { get; set; }
        public int kwhPriceInCents { get; set; }
        public object kwhPriceRange { get; set; }
        public int renewablePercentage { get; set; }
        public string indicator { get; set; }
        public string __typename { get; set; }
    }

    public class ForecastPeriod
    {
        public DateTime start { get; set; }
        public int kwhPriceInCents { get; set; }
        public object kwhPriceRange { get; set; }
        public int renewablePercentage { get; set; }
        public string indicator { get; set; }
        public string __typename { get; set; }
    }

    public class MeterWindow
    {
        public string usageType { get; set; }
        public List<object> forecastInformation { get; set; }
        public CurrentPeriod currentPeriod { get; set; }
        public List<PreviousPeriod> previousPeriods { get; set; }
        public List<ForecastPeriod> forecastPeriods { get; set; }
        public string __typename { get; set; }
    }

    public class SitePricing
    {
        public string remark { get; set; }
        public string solarRemark { get; set; }
        public object spikeRemark { get; set; }
        public List<MeterWindow> meterWindows { get; set; }
        public string __typename { get; set; }
    }

    public class Address
    {
        public string state { get; set; }
        public string postcode { get; set; }
        public string __typename { get; set; }
    }

    public class CustomerInfo
    {
        public Address address { get; set; }
        public string fullName { get; set; }
        public List<string> featureFlags { get; set; }
        public string __typename { get; set; }
    }

    public class Settings
    {
        public CustomerInfo customerInfo { get; set; }
        public string __typename { get; set; }
    }

    public class CostDiff
    {
        public string text { get; set; }
        public string indicator { get; set; }
        public string __typename { get; set; }
    }

    public class RenewableGridComparison
    {
        public string text { get; set; }
        public string indicator { get; set; }
        public string __typename { get; set; }
    }

    public class UsageDiff
    {
        public string text { get; set; }
        public string indicator { get; set; }
        public string __typename { get; set; }
    }

    public class PeriodSummary
    {
        public CostDiff costDiff { get; set; }
        public int costInCents { get; set; }
        public RenewableGridComparison renewableGridComparison { get; set; }
        public int renewablePercentage { get; set; }
        public UsageDiff usageDiff { get; set; }
        public int usageKwh { get; set; }
        public string usageType { get; set; }
        public string __typename { get; set; }
    }

    public class StackedUsage
    {
        public object controlled { get; set; }
        public double feedIn { get; set; }
        public double general { get; set; }
        public string __typename { get; set; }
    }

    public class FeedIn
    {
        public DateTime end { get; set; }
        public double kwh { get; set; }
        public DateTime start { get; set; }
        public string __typename { get; set; }
        public AveragePriceDiff averagePriceDiff { get; set; }
        public int averagePriceInCents { get; set; }
        public double carbonDisplaced { get; set; }
        public CarbonDisplacedDiff carbonDisplacedDiff { get; set; }
        public EarningsDiff earningsDiff { get; set; }
        public int earningsInCents { get; set; }
        public SuppliedDiff suppliedDiff { get; set; }
        public double suppliedKwh { get; set; }
        public string usageType { get; set; }
        public List<PricePeriod> pricePeriods { get; set; }
    }

    public class General
    {
        public DateTime end { get; set; }
        public double kwh { get; set; }
        public DateTime start { get; set; }
        public string __typename { get; set; }
        public CostDiff costDiff { get; set; }
        public int costInCents { get; set; }
        public RenewableGridComparison renewableGridComparison { get; set; }
        public int renewablePercentage { get; set; }
        public UsageDiff usageDiff { get; set; }
        public double usageKwh { get; set; }
        public string usageType { get; set; }
        public AveragePriceDiff averagePriceDiff { get; set; }
        public int averagePriceInCents { get; set; }
        public List<PricePeriod> pricePeriods { get; set; }
    }

    public class UsagePeriods
    {
        public object controlled { get; set; }
        public List<FeedIn> feedIn { get; set; }
        public List<General> general { get; set; }
        public string __typename { get; set; }
    }

    public class SuppliedDiff
    {
        public string text { get; set; }
        public string indicator { get; set; }
        public string __typename { get; set; }
    }

    public class Combined
    {
        public CostDiff costDiff { get; set; }
        public int costInCents { get; set; }
        public int renewablePercentage { get; set; }
        public RenewableGridComparison renewableGridComparison { get; set; }
        public SuppliedDiff suppliedDiff { get; set; }
        public double suppliedKwh { get; set; }
        public UsageDiff usageDiff { get; set; }
        public string usageType { get; set; }
        public double usageKwh { get; set; }
        public string __typename { get; set; }
    }

    public class AveragePriceDiff
    {
        public string text { get; set; }
        public string indicator { get; set; }
        public string __typename { get; set; }
    }

    public class CarbonDisplacedDiff
    {
        public string text { get; set; }
        public string indicator { get; set; }
        public string __typename { get; set; }
    }

    public class EarningsDiff
    {
        public string text { get; set; }
        public string indicator { get; set; }
        public string __typename { get; set; }
    }

    public class PricePeriod
    {
        public DateTime end { get; set; }
        public int kwhPriceInCents { get; set; }
        public int renewablePercentage { get; set; }
        public DateTime start { get; set; }
        public string __typename { get; set; }
    }

    public class UsageSummaries
    {
        public Combined combined { get; set; }
        public object controlled { get; set; }
        public FeedIn feedIn { get; set; }
        public General general { get; set; }
        public string __typename { get; set; }
    }

    public class BillingDay
    {
        public string marketDate { get; set; }
        public StackedUsage stackedUsage { get; set; }
        public UsagePeriods usagePeriods { get; set; }
        public UsageSummaries usageSummaries { get; set; }
        public string __typename { get; set; }
    }

    public class Snapshots
    {
        public PeriodSummary periodSummary { get; set; }
        public List<BillingDay> billingDays { get; set; }
        public string __typename { get; set; }
        public string siteStatus { get; set; }
        public LifetimeSavingSummary lifetimeSavingSummary { get; set; }
    }

    public class Data
    {
        public string whoami { get; set; }
        public SitePricing sitePricing { get; set; }
        public Settings settings { get; set; }
        public Snapshots snapshots { get; set; }
    }

    public class LifetimeSavingSummary
    {
        public double averagePriceInCents { get; set; }
        public AveragePriceDiff averagePriceDiff { get; set; }
        public string __typename { get; set; }
    }

  

    public class AmberGraphData
    {
        public Data data { get; set; }
    }

    public class AmberGraphDataParsed
    {
        public AmberGraphData LivePrice { get; set; }
        public AmberGraphData Usage { get; set; }
    }

    public class AmberServerResponse
    {
        public string LivePrice { get; set; }
        public string Usage { get; set; }
    }

    public class AmberPeriod
    {
        public DateTime End { get; set; }
        public double Kwh { get; set; }
        public DateTime Start { get; set; }
        public int KwhPriceInCents { get; set; }       
        public int RenewablePercentage { get; set; }
        public double ActualPrice
        {
            get
            {
                return KwhPriceInCents * Kwh;
            }
        }
    }

    public class AmberDay
    {
        public DateTime Start { get; set; }
        public double Kwh { get; set; }
        public double ActualPriceInCents { get; set; }
        public List<AmberPeriod> Periods { get; set; } = new List<AmberPeriod>();
    }

    public class AmberPriceComposed
    {
        public List<AmberDay> Days { get; set; }
        public double CurrentPrice { get; set; }
        public int RenewablePercentage { get; set; }

    }
}
