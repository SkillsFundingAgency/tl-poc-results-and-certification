using FluentValidation.Results;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model
{
    public class ValidationError
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public int RowNum { get; set; }
        public string RawRow { get; set; } 

        public ValidationResult ValidationResult { get; set; }
        public string ErrorMessage { get { return $"Row: {RowNum}, Column: {FieldName} has invalid data: {FieldValue}"; } }
    }
}
