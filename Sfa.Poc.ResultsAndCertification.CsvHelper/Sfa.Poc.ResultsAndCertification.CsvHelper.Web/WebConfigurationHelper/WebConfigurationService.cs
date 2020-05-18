using Sfa.Poc.ResultsAndCertification.CsvHelper.Models.Configuration;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Web.WebConfigurationHelper;

namespace Sfa.Tl.ResultsAndCertification.Web.WebConfigurationHelper
{
    public class WebConfigurationService : IWebConfigurationService
    {
        public readonly ResultsAndCertificationConfiguration _configuration;
        public WebConfigurationService(ResultsAndCertificationConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetFeedbackEmailAddress()
        {
            return string.Empty; //_configuration.FeedbackEmailAddress;
        }

        public string GetSignOutPath()
        {
            return string.Empty;
            //return _configuration.DfeSignInSettings.SignOutEnabled ? RouteConstants.SignOutDsi : RouteConstants.SignOut;
        }
    }
}
