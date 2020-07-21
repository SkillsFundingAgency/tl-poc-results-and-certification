using CsvHelper;
using CsvHelper.Configuration.Attributes;
using FluentValidation.Results;
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
        
        [Ignore]
        public bool IsFileReadDirty { get; set; }

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

        public void AddErrors(int rownum, ReadingContext context, ValidationResult validationResult = null)
        {
            foreach(var err in validationResult.Errors)
            {
                var error = new ValidationError
                {
                    FieldName = err.PropertyName,
                    FieldValue = err.AttemptedValue.ToString(),
                    RowNum = rownum,
                    RawRow = context.RawRecord,
                    ErrorMessage = err.ErrorMessage
                };

                ValidationErrors.Add(error);
            }
        }

        public void AddStage3Error(int rowNum, string message)
        {
            ValidationErrors.Add(new ValidationError
            {
                FieldName = "NA",
                FieldValue = "NA",
                RawRow = "TODO",
                RowNum = rowNum,
                ErrorMessage = message
            });
        }

        public void AddError(int rownum, ReadingContext context, ValidationResult validationResult = null)
        {
            foreach (var err in validationResult.Errors)
            {
                var error = new ValidationError
                {
                    FieldName = err.PropertyName,
                    FieldValue = err.AttemptedValue.ToString(),
                    RowNum = rownum,
                    RawRow = context.RawRecord,
                    ErrorMessage = err.ErrorMessage
                };

                ValidationErrors.Add(error);
            }
        }

        public ValidationError BuildError(ReadingContext context, string message = "", ValidationResult validationResult = null)
        {
            var fieldIndex = context.CurrentIndex;
            var error = new ValidationError
            {
                FieldName = context.HeaderRecord[fieldIndex],
                FieldValue = context.Record[fieldIndex],
                RowNum = context.Row,
                RawRow = context.RawRecord,
                ValidationResult = validationResult
            };

            return error;
        }
    }
}
