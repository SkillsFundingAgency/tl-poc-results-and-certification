
namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Api.Infrastructure
{
    public class OkResponse<TResponseType> : ApiResponse
    {
        public TResponseType Result { get; }

        public OkResponse(TResponseType result) : base(200)
        {
            Result = result;
        }
    }
}
