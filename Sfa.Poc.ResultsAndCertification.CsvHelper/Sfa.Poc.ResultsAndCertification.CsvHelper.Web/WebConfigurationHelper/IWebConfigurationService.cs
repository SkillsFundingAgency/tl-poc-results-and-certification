namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Web.WebConfigurationHelper
{
    public interface IWebConfigurationService
    {
        string GetFeedbackEmailAddress();

        string GetSignOutPath();
    }
}
