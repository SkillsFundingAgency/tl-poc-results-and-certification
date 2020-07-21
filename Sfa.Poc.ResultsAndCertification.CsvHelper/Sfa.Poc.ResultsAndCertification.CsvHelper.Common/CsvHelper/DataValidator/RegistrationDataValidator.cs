using FluentValidation;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.DataValidator
{
    public class RegistrationDataValidator : AbstractValidator<RegistrationCsvRecord>
    {
        public RegistrationDataValidator()
        {
            // Uln
            RuleFor(r => r.Uln)
                .Required()
                .MustBeNumberWithLength(10);

            // Firstname
            RuleFor(r => r.FirstName)
                .Required()
                .MaxStringLength(10);

            // Lastname
            RuleFor(r => r.LastName)
                .Required()
                .MaxStringLength(10);

            // DateofBirth
            RuleFor(r => r.DateOfBirth)
                .Required()
                .ValidDate()
                .NotFutureDate();

            // Ukprn
            RuleFor(r => r.Ukprn)
                .Required()
                .MustBeNumberWithLength(8);

            // Startdate
            RuleFor(r => r.StartDate)
                .Required()
                .ValidDate();

            // Core
            RuleFor(r => r.Core)
                .Required();
        }
    }
}
