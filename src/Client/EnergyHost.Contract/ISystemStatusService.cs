using System.Threading.Tasks;
using EnergyHost.Model.EnergyModels.Status.Base;

namespace EnergyHost.Contract
{
    public interface ISystemStatusService
    {
        Task SendStatus(StatusBase status);
    }
}