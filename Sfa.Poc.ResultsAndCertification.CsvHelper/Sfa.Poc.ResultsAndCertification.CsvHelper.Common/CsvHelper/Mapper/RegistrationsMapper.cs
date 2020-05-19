using CsvHelper.Configuration;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Mapper
{
    public class RegistrationsMapper : ClassMap<Registration>
    {
        //Note: Not in use as we are not bulk reading.
        public RegistrationsMapper()
        {
            Map(m => m.Uln).Name(Constants.CsvHeaders.Uln);
            Map(m => m.Ukprn).Name(Constants.CsvHeaders.Ukprn);
            Map(m => m.StartDate).Name(Constants.CsvHeaders.StartDate);
            Map(m => m.Core).Name(Constants.CsvHeaders.Core);
            Map(m => m.Specialism1).Name(Constants.CsvHeaders.Specialism1);
            Map(m => m.Specialism2).Name(Constants.CsvHeaders.Specialism2);
        }
    }
}
