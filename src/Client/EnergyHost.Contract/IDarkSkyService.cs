using System.Threading.Tasks;
using DarkSky.Models;

namespace EnergyHost.Contract
{
    public interface IDarkSkyService
    {
        Task<Forecast> Get();

        Task<(double temp, double humid, double pressure,
            double wind, double minToday, double maxToday,
            double minTomorrow, double maxTomorrow)> GetDetail();
    }
}