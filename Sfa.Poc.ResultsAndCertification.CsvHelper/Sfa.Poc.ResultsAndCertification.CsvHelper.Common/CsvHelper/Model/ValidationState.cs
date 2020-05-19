using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model
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
                //if (typeof(T) == typeof(DateTime))
                //{
                //    // Note: This may be an expensive. Need inputs if any alternative.
                //    var csvStartDate = csv.GetField<string>(fieldName);
                //    var startDate = DateTime.ParseExact(csvStartDate, "ddMMyyyy", CultureInfo.InvariantCulture);
                //    return (T)Convert.ChangeType(startDate, typeof(T));
                //}

                var result = csv.GetField<T>(fieldName);

                //var validationContext = new ValidationContext(result);
                //var validationResult = new List<ValidationResult>();

                //Validator.TryValidateProperty(result, validationContext, validationResult);
                //if (validationResult.Count > 0)
                //{
                //    throw new Exception(validationResult.First().ErrorMessage);
                //}

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

        private static ValidationError BuildError(ReadingContext context, string message = "")
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
