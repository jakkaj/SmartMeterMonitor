using System.Threading.Tasks;

namespace EnergyHost.Contract
{
    public interface IDataLoggerService
    {
        Task Start();
    }
}