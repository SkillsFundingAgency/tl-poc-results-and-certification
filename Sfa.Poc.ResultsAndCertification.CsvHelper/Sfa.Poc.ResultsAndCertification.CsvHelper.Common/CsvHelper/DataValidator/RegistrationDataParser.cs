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
        public Registration GetErrorObjct(string message)
        {
            return new Registration
            {
                ValidationErrors = new List<ValidationError>
                {
                    new ValidationError { ErrorMessage = message }
                },
                IsFileReadDirty = true
            };
        }

        public Registration Parse(BaseModel model, int rownum)
        {
            if (model is RegistrationCsvRecord reg)
            {
                if (!reg.IsValid)
                    return new Registration { ValidationErrors = new List<ValidationError>(reg.ValidationErrors) };

                DateTime.TryParseExact(reg.DateOfBirth.Trim(), "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob);
                DateTime.TryParseExact(reg.StartDate.Trim(), "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate);

                return new Registration
                {
                    Uln = int.Parse(reg.Uln.Trim()),
                    FirstName = reg.FirstName.Trim(),
                    LastName = reg.LastName.Trim(),
                    DateOfBirth = dob,
                    Ukprn = reg.Ukprn.ToLong(),
                    StartDate = startDate,
                    Core = reg.Core.Trim(),
                    Specialisms = reg.Specialisms.Trim().Split(',').Where(s => !string.IsNullOrEmpty(s)),
                    RowNum = rownum,
                    ValidationErrors = new List<ValidationError>(reg.ValidationErrors)
                };
            }

            return null;
        }
    }
}
