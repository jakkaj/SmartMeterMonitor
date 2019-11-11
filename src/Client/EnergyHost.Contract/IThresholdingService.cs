using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnergyHost.Contract
{
    public interface IThresholdingService
    {
        Task RunChecks(Dictionary<string, object> data);
    }
}