using System.Threading.Tasks;
using EnergyHost.Model.DataModels;

namespace EnergyHost.Contract
{
    public interface IAmberServiceV2
    {
        Task<AmberGraphDataParsed> Get();
    }
}