using System.Threading.Tasks;
using EnergyHost.Model.EnergyModels;

namespace EnergyHost.Contract
{
    public interface IEnergyFuturesService
    {
        Task<EnergryFutures> Get();
    }
}