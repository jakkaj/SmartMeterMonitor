using System.Threading.Tasks;
using EnergyHost.Model.EnergyModels;

namespace EnergyHost.Contract
{
    public interface IABBService
    {
        Task<ABBDevice> Get();
        Task<ABBSunspec> GetModbus();
    }
}