using System.Collections.Generic;
using System.Linq;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Models.BulkUpload
{
    public class BulkUploadResponse
    {
        public BulkUploadResponse ()
        {
            BulkUploadErrors = new List<BulkUploadError>();
        }

        public bool IsSuccess { get; set; }
        public BulkUploadStats BulkUploadStats { get; set; }
        public List<BulkUploadError> BulkUploadErrors { get; set; }

        public bool HasAnyErrors { get { return BulkUploadErrors.Any(); } }
    }

    public class BulkUploadStats
    {
        public int NewRecordsCount { get; set; }
        public int UpdatedRecordsCount { get; set; }
        public int DuplicateRecordsCount { get; set; }
    }
}
