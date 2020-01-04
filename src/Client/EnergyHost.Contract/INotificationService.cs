using System.Threading.Tasks;

namespace EnergyHost.Contract
{
    public interface INotificationService
    {
        Task SendNotification(string text);
    }
}