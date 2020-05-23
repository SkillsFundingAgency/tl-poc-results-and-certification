using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper
{
    public interface IDataParser<out T> where T : class
    {
        T Parse(BaseModel model);
    }
}
