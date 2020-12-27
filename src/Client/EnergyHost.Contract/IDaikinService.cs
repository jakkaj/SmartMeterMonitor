using System.Collections.Specialized;
using System.Threading.Tasks;
using EnergyHost.Model.DataModels;
using EnergyHost.Model.EnergyModels.Status;

namespace EnergyHost.Contract
{
    public interface IDaikinService
    {
        Task<NameValueCollection> GetSensors();
        Task<DaikinSettings> GetControlInfo();
        Task SetControlInfo(DaikinSettings settings);
        Task<bool> PowerOff();
    }
}