﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnergyHost.Contract
{
    public interface IInfluxService
    {
        Task Write(string db, string measurement, Dictionary<string, object> data);
        Task<bool> Write(string db, string measurement, IReadOnlyDictionary<string, object> data, IReadOnlyDictionary<string, string> tags = null, DateTime? utcTimeStamp = null);
    }
}