using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Helpers;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.DataValidator
{
    public class RegistrationDataParser : IDataParser<Registration>
    {
        public Registration Parse(BaseModel model)
        {
            if (model is RegistrationCsvRecord reg)
            {
                return new Registration
                {
                    Uln = reg.Uln.ToLong(),
                    FirstName = reg.FirstName,
                    LastName = reg.LastName,
                    DateOfBirth = reg.DateOfBirth,
                    Ukprn = reg.Ukprn.ToLong(),
                    StartDate = reg.StartDate,
                    Core = reg.Core,
                    Specialism1 = reg.Specialism1,
                    Specialism2 = reg.Specialism2
                };
            }
            return null;
        }
    }
}
