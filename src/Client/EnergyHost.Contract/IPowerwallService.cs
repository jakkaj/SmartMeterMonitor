using System.Threading.Tasks;
using EnergyHost.Model.DataModels;

namespace EnergyHost.Contract
{
    public interface IPowerwallService
    {
        Task<Powerwall> GetPowerwall();
        Task<double> GetUsedToday();
        Task<int> GetReservePercent();
        Task<string> SetReservePercent(int reserve);
    }
}