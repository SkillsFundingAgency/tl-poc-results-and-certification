namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Models.BulkUpload
{
    public class BulkUploadError
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string ErrorMessage { get; set; }
    }
}
