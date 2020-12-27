using System.Threading.Tasks;
using EnergyHost.Model.DataModels;


namespace EnergyHost.Services.Contract
{
    public interface INetatmoService
    {
        Task<NetatmoData> Get();
    }
}