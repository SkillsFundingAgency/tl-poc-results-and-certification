
namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Models.Configuration
{
    public class ResultsAndCertificationConfiguration
    {
        public string BlobStorageConnectionString { get; set; }

        public string SqlConnectionString { get; set; }

        public DfeSignInSettings DfeSignInSettings { get; set; }

        public LearningRecordServiceSettings LearningRecordServiceSettings { get; set; }

        public string ResultsAndCertificationInternalApiUri { get; set; }

        public string KeyVaultUri { get; set; }
    }
}
