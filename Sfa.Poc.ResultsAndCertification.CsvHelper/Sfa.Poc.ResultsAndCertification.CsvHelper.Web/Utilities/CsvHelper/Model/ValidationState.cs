using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Web.Utilities.CsvHelper.Model
{
    public class ValidationState
    {
        public ValidationState()
        {
            ValidationErrors = new List<ValidationError>();
        }

        public IList<ValidationError> ValidationErrors;
        public bool IsValid { get { return ValidationErrors.Count() == 0; } }

        internal T Validate<T>(CsvReader csv, string fieldName)
        {
            try
            {
                if (typeof(T) == typeof(DateTime))
                {
                    // Note: This may be an expensive. Need inputs if any alternative.
                    var csvStartDate = csv.GetField<string>(fieldName);
                    var startDate = DateTime.ParseExact(csvStartDate, "ddMMyyyy", CultureInfo.InvariantCulture);
                    return (T)Convert.ChangeType(startDate, typeof(T));
                }

                // TODO: How to know what is the mandatory field.
                var result = csv.GetField<T>(fieldName);
                return result;
            }
            catch (CsvHelperException e)
            {
                ValidationErrors.Add(BuildError(e.ReadingContext));
                return default;
            }
            catch (Exception ex)
            {
                ValidationErrors.Add(BuildError(csv.Context));
                return default;
            }
        }

        private static ValidationError BuildError(ReadingContext context)
        {
            var fieldIndex = context.CurrentIndex;
            var error = new ValidationError
            {
                FieldName = context.HeaderRecord[fieldIndex],
                FieldValue = context.Record[fieldIndex],
                RowNum = context.Row,
                RawRow = context.RawRecord
            };

            return error;
        }
    }
}
