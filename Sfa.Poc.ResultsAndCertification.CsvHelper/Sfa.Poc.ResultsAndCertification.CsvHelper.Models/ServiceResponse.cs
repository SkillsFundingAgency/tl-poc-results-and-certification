
namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Models
{
    public class ServiceResponse<TResponseType>
    {
        public ServiceResponse(TResponseType value)
        {
            Value = value;
        }

        public ServiceResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public ServiceResponse() { }
        public TResponseType Value { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; }
    }
}
