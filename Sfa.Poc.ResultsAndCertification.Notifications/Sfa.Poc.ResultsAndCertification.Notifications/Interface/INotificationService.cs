using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.Notifications.Interface
{
    public interface INotificationService
    {
        Task SendNotification(string templateName, string toAddress, dynamic tokens);
    }
}
