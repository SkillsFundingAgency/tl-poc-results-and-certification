namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.DataValidator
{
    public class ValidationMessages
    {
        public const string Required = "{0} is required.";
        public const string MustBeNumberWithLength = "{0} must be a number with {1} digits.";
        public const string StringLength = "{0} cannot be greater than {1} characters.";
        public const string MustBeValidDate = "{0} should be a valid date in DDMMYYYY format.";
        public const string DateNotinFuture = "{0} should be not be a future date.";

    }
}
