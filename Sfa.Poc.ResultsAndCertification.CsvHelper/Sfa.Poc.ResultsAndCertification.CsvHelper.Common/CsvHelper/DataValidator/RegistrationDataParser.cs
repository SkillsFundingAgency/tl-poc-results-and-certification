using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Helpers;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.DataValidator
{
    public class RegistrationDataParser : IDataParser<Registration>
    {
        public Registration Parse(BaseModel model, int rownum)
        {
            if (model is RegistrationCsvRecord reg)
            {
                if (!reg.IsValid)
                    return new Registration { ValidationErrors = new List<ValidationError>(reg.ValidationErrors) };

                return new Registration
                {
                    Uln = reg.Uln.ToLong(),
                    FirstName = reg.FirstName,
                    LastName = reg.LastName,
                    DateOfBirth = reg.DateOfBirth,
                    Ukprn = reg.Ukprn.ToLong(),
                    StartDate = reg.StartDate,
                    Core = reg.Core,
                    Specialisms = reg.Specialisms.Split(',').Where(s => !string.IsNullOrEmpty(s)),
                    RowNum = rownum,
                    ValidationErrors = new List<ValidationError>(reg.ValidationErrors)
                };
            }

            return null;
        }
    }
}
