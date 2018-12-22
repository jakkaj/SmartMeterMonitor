using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Helpers
{
    public class PowerBIModel
    {
        public double kwh { get; set; }
        public string measuretime { get; set; }
        public double kwhday { get; set; }
        public double AverageSoFarToday { get; set; }
        public double AverageSoFarYesterday { get; set; }
        public double AverageLast24Hours { get; set; }
        public double AverageYesterday { get; set; }
       
        public int maxvalue { get; set; }
    }
}
