using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model
{
    public class ImportResponseRecord<TResponseModel> : ResponseBaseModel where TResponseModel : class, new()
    {
        public IList<TResponseModel> ResponseModel { get; set; }
    }
}
