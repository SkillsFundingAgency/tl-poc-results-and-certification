using System.Threading.Tasks;
using System.Collections.Generic;
using Notify.Interfaces;
using Sfa.Poc.ResultsAndCertification.Notifications.Configuration;
using Sfa.Poc.ResultsAndCertification.Notifications.Interface;

namespace Sfa.Poc.ResultsAndCertification.Notifications.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ResultsAndCertificationConfiguration _configuration;
        private readonly IAsyncNotificationClient _notificationClient;

        public NotificationService(ResultsAndCertificationConfiguration configuration, IAsyncNotificationClient notificationClient)
        {
            _configuration = configuration;
            _notificationClient = notificationClient;
        }

        public async Task SendNotification(string templateName, string toAddress, dynamic tokens)
        {
            var templateId = templateName == Constants.TlevelsQueryTemplateName ? Constants.TlevelsQueryTemplateId : "";
            var personalisationTokens = new Dictionary<string, dynamic>();
            
            foreach (var property in tokens.GetType().GetProperties())
            {
                personalisationTokens[property.Name] = property.GetValue(tokens);
            }

            var emailNotificationResponse =await _notificationClient.SendEmailAsync(
                emailAddress: toAddress,
                templateId: templateId,
                personalisation: personalisationTokens);

            var result = emailNotificationResponse;
        }
    }
}
