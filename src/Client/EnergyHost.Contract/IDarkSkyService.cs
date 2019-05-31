using System.Threading.Tasks;
using DarkSky.Models;

namespace EnergyHost.Contract
{
    public interface IDarkSkyService
    {
        Task<Forecast> Get();
    }
}