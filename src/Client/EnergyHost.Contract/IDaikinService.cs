using System.Collections.Specialized;
using System.Threading.Tasks;

namespace EnergyHost.Contract
{
    public interface IDaikinService
    {
        Task<NameValueCollection> GetSensors();
    }
}