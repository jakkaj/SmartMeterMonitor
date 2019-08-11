using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EnergyHost.Model.DataModels;

namespace EnergyHost.Contract
{
    public interface IInfluxService
    {
        Task Write(string db, string measurement, Dictionary<string, object> data);
        Task<bool> Write(string db, string measurement, IReadOnlyDictionary<string, object> data, IReadOnlyDictionary<string, string> tags = null, DateTime? utcTimeStamp = null);

        Task<bool> WriteObject(string db, string measurement, object data,
            IReadOnlyDictionary<string, string> tags = null, DateTime? utcTimeStamp = null);

        Task<InfluxResult> Query(string db, string query);
    }
}