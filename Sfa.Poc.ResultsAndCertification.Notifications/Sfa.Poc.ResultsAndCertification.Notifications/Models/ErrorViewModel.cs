using System;

namespace Sfa.Poc.ResultsAndCertification.Notifications.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
