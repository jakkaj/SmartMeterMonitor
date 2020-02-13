using System.Threading.Tasks;
using EnergyHost.Model.EnergyModels;

namespace EnergyHost.Contract
{
    public interface IAmberService
    {
        Task<AmberData> Get(string postCode);
        Task<AmberLogin> Login();
        Task<AmberUsage> GetUsage();
    }
}