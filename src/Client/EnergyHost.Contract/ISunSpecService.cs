using System.Threading.Tasks;
using EnergyHost.Model.EnergyModels;

namespace EnergyHost.Contract
{
    public interface ISunSpecService
    {
        Task<ABBDevice> Get();
        Task<SolarEdgeSunSpec> GetModbus();
    }
}