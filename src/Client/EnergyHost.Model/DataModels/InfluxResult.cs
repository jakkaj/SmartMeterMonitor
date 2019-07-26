using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace EnergyHost.Model.DataModels
{
    public class InfluxSeries
    {
        [JsonProperty("name")]
        string Name { get; set; }

        [JsonProperty("columns")]
        public List<string> Columns { get; set; }

        [JsonProperty("values")]
        public List<List<object>> Values { get; set; }
    }

    public class Result
    { 
        [JsonProperty("statement_id")]
        public int StatementId { get; set; }
        [JsonProperty("series")]
        public List<InfluxSeries> Series { get; set; }
    }

    public class InfluxResult
    {
        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }
}
