using FluentValidation;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Helpers;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.DataValidator
{
    public class RegistrationDataValidator : AbstractValidator<RegistrationCsvRecord>
    {
        public RegistrationDataValidator()
        {
            RuleFor(r => r.Uln)
                .Required();
            
            RuleFor(r => r.Uln)
                .LenthMustBe(10)
                .When(r => !string.IsNullOrWhiteSpace(r.Uln));

            //RuleFor(r => r.Uln)
            //    .Must(r => r.Length == 10)
            //        .WithErrorCode("Size")
            //        .WithMessage("Uln length not as expected.")
            //    .When(r => !string.IsNullOrWhiteSpace(r.Uln));

            //RuleFor(r => r.Uln)
            //    .Must(r => int.TryParse(r, out int result))
            //        .WithErrorCode("Type")
            //        .WithMessage("Uln is not expected type.")
            //    .When(r => !string.IsNullOrWhiteSpace(r.Uln) && r.Uln.Length == 10);

            //RuleFor(r => r.FirstName)
            //    .NotEmpty()
            //        .WithErrorCode("Mandatory")
            //        .WithMessage("FirstName is missing");

            //RuleFor(r => r.LastName)
            //    .NotEmpty()
            //        .WithErrorCode("Mandatory")
            //        .WithMessage("LastName is missing");

            //RuleFor(r => r.DateOfBirth)
            //    .NotEmpty()
            //        .WithErrorCode("Mandatory")
            //        .WithMessage("DateOfBirth is missing")
            //    .Must(dto => dto.IsDateTimeWithFormat())
            //        .WithErrorCode("InvalidFormat")
            //        .WithMessage("DateOfBirth - Invalid Format");

            //RuleFor(r => r.Ukprn)
            //    .Must(r => r.ToString().Length == 8)
            //        .WithErrorCode("InvalidLength")
            //        .WithMessage("Ukprn - Invalid length, must be 8 numbers long");

            //RuleFor(dto => dto.StartDate)
            //    .NotEmpty()
            //        .WithErrorCode("Mandatory")
            //        .WithMessage("StartDate is missing")
            //    .Must(dto => dto.IsDateTimeWithFormat())
            //        .WithErrorCode("InvalidFormat")
            //        .WithMessage("StartDate - Invalid Format");

            //RuleFor(r => r.Core)
            //    .NotEmpty()
            //        .WithErrorCode("Mandatory")
            //        .WithMessage("Core is missing");
        }
    }
}
