using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnergyHost.Contract
{
    public interface IInfluxService
    {
        Task Write(string db, string measurement, Dictionary<string, object> data);
    }
}