using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Models.BulkUpload
{
    public class BulkUploadError
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string ErrorMessage { get; set; }
    }
}
