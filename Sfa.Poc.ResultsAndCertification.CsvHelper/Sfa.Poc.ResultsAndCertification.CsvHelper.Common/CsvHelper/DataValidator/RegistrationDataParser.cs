using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Helpers;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
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

                DateTime.TryParseExact(reg.DateOfBirth, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob);
                DateTime.TryParseExact(reg.StartDate, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate);

                return new Registration
                {
                    Uln = int.Parse(reg.Uln),
                    FirstName = reg.FirstName,
                    LastName = reg.LastName,
                    DateOfBirth = dob,
                    Ukprn = reg.Ukprn.ToLong(),
                    StartDate = startDate,
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
