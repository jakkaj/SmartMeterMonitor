using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace EnergyHost.Model.EnergyModels
{


    public class Datum
    {
        public double value { get; set; }
        public DateTime timestamp { get; set; }
    }

    public class Readings
    {
        public List<Datum> data { get; set; }
        public int decimal_precision { get; set; }
        public string title { get; set; }
        public string units { get; set; }
        public string @class { get; set; }
        public int priority { get; set; }
    }


    public class Datastreams
    {
        public Readings m101_1_A { get; set; }
        public Readings m101_1_DCW { get; set; }
        public Readings m101_1_Hz { get; set; }
        public Readings m101_1_PF { get; set; }
        public Readings m101_1_PhVphA { get; set; }
        public Readings m101_1_PowerPeakAbs { get; set; }
        public Readings m101_1_PowerPeakToday { get; set; }
        public Readings m101_1_TmpCab { get; set; }
        public Readings m101_1_TmpOt { get; set; }
        public Readings m101_1_W { get; set; }
        public Readings m101_1_WH { get; set; }
        public Readings m160_1_DCA_1 { get; set; }
        public Readings m160_1_DCA_2 { get; set; }
        public Readings m160_1_DCV_1 { get; set; }
        public Readings m160_1_DCV_2 { get; set; }
        public Readings m160_1_DCW_1 { get; set; }
        public Readings m160_1_DCW_2 { get; set; }
        public Readings m64061_1_Booster_Tmp { get; set; }
        public Readings m64061_1_DayWH { get; set; }
        public Readings m64061_1_ILeakDcAc { get; set; }
        public Readings m64061_1_ILeakDcDc { get; set; }
        public Readings m64061_1_Isolation_Ohm1 { get; set; }
        public Readings m64061_1_MonthWH { get; set; }
        public Readings m64061_1_VBulk { get; set; }
        public Readings m64061_1_VBulkMid { get; set; }
        public Readings m64061_1_WeekWH { get; set; }
        public Readings m64061_1_YearWH { get; set; }
    }

    public class DeviceFeed
    {
        public List<string> feedIntervals { get; set; }
        public string description { get; set; }
        public Datastreams datastreams { get; set; }
    }

    public class Feeds
    {
        [JsonProperty("ser4:125721-3G96-1417")]
        public DeviceFeed Feed { get; set; }
    }

    public class ABBDevice
    {
        public Feeds feeds { get; set; }
    }
}
