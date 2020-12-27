using System.Threading.Tasks;

namespace Smart.Helpers.Contracts
{
    public interface IDatabaseService
    {
        Task<double> AverageSoFarToday();
        Task<double> AverageSoFarYesterday();
        Task<double> AverageYesterday();
        Task<double> AverageLast24Hours();
    }
}