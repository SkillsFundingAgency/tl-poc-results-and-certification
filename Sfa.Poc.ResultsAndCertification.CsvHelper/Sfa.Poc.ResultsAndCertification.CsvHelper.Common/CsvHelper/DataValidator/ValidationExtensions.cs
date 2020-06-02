using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.DataValidator
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> Required<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("{PropertyName} is required.");
        }

        public static IRuleBuilderOptions<T, string> LenthMustBe<T>(this IRuleBuilder<T, string> ruleBuilder, int length)
        {
            return ruleBuilder
                .Must(r => r.Length == 10)
                .WithMessage("{PropertyName} length must be {length}.");
        }


        //public static IRuleBuilderOptions<T, string> ValidDate<T>(this IRuleBuilder<T, string> ruleBuilder)
        //{
        //    return ruleBuilder
        //        .Must(x => Utilities.IsAValidDate(x))
        //        .WithMessage(x => string.Format(ValidationMessages.ValidDate, "{PropertyName}"));
        //}

        //public static IRuleBuilderOptions<T, string> MustBeFutureDate<T>(this IRuleBuilder<T, string> ruleBuilder)
        //{
        //    return ruleBuilder
        //        .Must(y => Utilities.ParseStringToDateTime(y) >= DateTime.Now.Date)
        //        .WithMessage("{PropertyName} must be in the future.");
        //}

        //public static IRuleBuilderOptions<T, string> NotFutureDate<T>(this IRuleBuilder<T, string> ruleBuilder)
        //{
        //    return ruleBuilder
        //        .Must(y => Utilities.ParseStringToDateTime(y) <= DateTime.Now)
        //        .WithMessage("{PropertyName} cannot be in the future.");
        //}

        //public static IRuleBuilderOptions<T, string> NotPastDate<T>(this IRuleBuilder<T, string> ruleBuilder)
        //{
        //    return ruleBuilder
        //        .Must(y => DateTime.Now.Date <= Utilities.ParseStringToDateTime(y))
        //        .WithMessage("{PropertyName} cannot be in the past.");
        //}

        //public static IRuleBuilderOptions<T, string> Valid24HourTime<T>(this IRuleBuilder<T, string> ruleBuilder)
        //{
        //    return ruleBuilder
        //        .Matches(Formats.TimeRegex)
        //        .WithMessage(x => string.Format(ValidationMessages.Valid24HourTime, "{PropertyName}"));
        //}

        //public static IRuleBuilderOptions<T, int> SelectRequired<T>(this IRuleBuilder<T, int> ruleBuilder)
        //{
        //    return ruleBuilder
        //        .GreaterThan(0)
        //        .WithMessage(x => string.Format(ValidationMessages.Required, "{PropertyName}"));
        //}

        //public static IRuleBuilderOptions<T, string> Required<T>(this IRuleBuilder<T, string> ruleBuilder)
        //{
        //    return ruleBuilder
        //        .Must(x => !string.IsNullOrWhiteSpace(x))
        //        .WithMessage(x => string.Format(ValidationMessages.Required, "{PropertyName}"));
        //}

        //public static IRuleBuilderOptions<T, string> ValidInteger<T>(this IRuleBuilder<T, string> ruleBuilder)
        //{
        //    return ruleBuilder
        //        .Must(x => int.TryParse(x, out int result))
        //        .WithMessage(x => string.Format(ValidationMessages.ValidInteger, "{PropertyName}"));
        //}

        //public static IRuleBuilderOptions<T, string> ValidDecimal<T>(this IRuleBuilder<T, string> ruleBuilder)
        //{
        //    return ruleBuilder
        //        .Must(x => decimal.TryParse(x, out decimal result))
        //        .WithMessage(x => string.Format(ValidationMessages.ValidDecimal, "{PropertyName}"));
        //}

        //public static IRuleBuilderOptions<T, string> DecimalWithTwoDecimalSpaces<T>(this IRuleBuilder<T, string> ruleBuilder)
        //{
        //    var pattern = @"^\d+(?:\.\d{1,2})?$";
        //    var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

        //    return ruleBuilder
        //        .Must(x => regex.Match(x).Length > 0)
        //        .WithMessage(x => string.Format(ValidationMessages.MaxDecimalPlacesExceeded, "{PropertyName}"));
        //}

        //public static IRuleBuilderOptions<T, string> DecimalStringLessThan<T>(this IRuleBuilder<T, string> ruleBuilder, decimal limit)
        //{
        //    return ruleBuilder
        //        .Must(x => decimal.TryParse(x, out decimal result) && result < limit)
        //        .WithMessage(x => string.Format(ValidationMessages.LessThan, "{PropertyName}", limit.ToString("N0")));
        //}

        //public static IRuleBuilderOptions<T, string> DecimalStringGreaterThan<T>(this IRuleBuilder<T, string> ruleBuilder, decimal limit)
        //{
        //    return ruleBuilder
        //        .Must(x => decimal.TryParse(x, out decimal result) && result > limit)
        //        .WithMessage(x => string.Format(ValidationMessages.GreaterThan, "{PropertyName}", limit.ToString("N0")));
        //}

        //public static IRuleBuilderOptions<T, string> ValidTimeXHours15<T>(this IRuleBuilder<T, string> ruleBuilder)
        //{
        //    return ruleBuilder
        //        .Matches("^([0-9]|[0-9][0-9]):(00|15|30|45)$")
        //        .WithMessage("{PropertyName} must be in hour/minute format with a 15 minute interval.");
        //}

        //public static IRuleBuilderOptions<T, string> ValidHoursDecimal<T>(this IRuleBuilder<T, string> ruleBuilder)
        //{
        //    var pattern = @"^(\d+(\.(0|00|25|5|50|75))?)|(\.(25|5|50|75))$";
        //    var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        //    return ruleBuilder
        //        .Must(x => regex.Match(x).Length == x.Length)
        //        .WithMessage("{PropertyName} must be a positive number and in multiples of 0.25.");
        //}

        //public static IRuleBuilderOptions<T, string> MaxStringLength<T>(this IRuleBuilder<T, string> ruleBuilder, int max)
        //{
        //    return ruleBuilder
        //        .Must(x => x == null || x.Length <= max)
        //        .WithMessage(string.Format(ValidationMessages.StringLength, "{PropertyName}", max.ToString()));
        //}

        //public static IRuleBuilderOptions<T, string> IntegerStringGreaterThan<T>(this IRuleBuilder<T, string> ruleBuilder, int limit)
        //{
        //    return ruleBuilder
        //        .Must(x => int.TryParse(x, out int result) && result > limit)
        //        .WithMessage(x => string.Format(ValidationMessages.GreaterThan, "{PropertyName}", limit.ToString("N0")));
        //}

        //public static IRuleBuilderOptions<T, string> IntegerStringGreaterThanOrEquals<T>(this IRuleBuilder<T, string> ruleBuilder, int limit)
        //{
        //    return ruleBuilder
        //        .Must(x => int.TryParse(x, out int result) && result >= limit)
        //        .WithMessage(x => string.Format(ValidationMessages.GreaterThanOrEquals, "{PropertyName}", limit.ToString("N0")));
        //}

        //public static IRuleBuilderOptions<T, string> IntegerStringLessThan<T>(this IRuleBuilder<T, string> ruleBuilder, int limit)
        //{
        //    return ruleBuilder
        //        .Must(x => int.TryParse(x, out int result) && result < limit)
        //        .WithMessage(x => string.Format(ValidationMessages.LessThan, "{PropertyName}", limit.ToString("N0")));
        //}

        //public static IRuleBuilderOptions<T, string> IntegerStringValueBetween<T>(this IRuleBuilder<T, string> ruleBuilder, decimal lowerBound, decimal upperBound)
        //{
        //    return ruleBuilder
        //        .Must(x => int.TryParse(x, out int result) && result >= lowerBound && result <= upperBound)
        //        .WithMessage(x => string.Format(ValidationMessages.Between, "{PropertyName}", lowerBound.ToString(), upperBound.ToString()));
        //}

        //public static IRuleBuilderOptions<T, List<string>> Required<T>(this IRuleBuilder<T, List<string>> ruleBuilder)
        //{
        //    return ruleBuilder
        //        .Must(x => x != null && x.Count > 0)
        //        .WithMessage(x => string.Format(ValidationMessages.Required, "{PropertyName}"));
        //}

        //public static IRuleBuilderOptions<T, List<int>> Required<T>(this IRuleBuilder<T, List<int>> ruleBuilder)
        //{
        //    return ruleBuilder
        //        .Must(x => x != null && x.Count > 0)
        //        .WithMessage(x => string.Format(ValidationMessages.Required, "{PropertyName}"));
        //}

        //public static IRuleBuilderOptions<T, int?> RequiredGreaterThan<T>(this IRuleBuilder<T, int?> ruleBuilder, int compareTo)
        //{
        //    return ruleBuilder
        //        .Must(x => x.HasValue && x.Value > compareTo)
        //        .WithMessage(x => string.Format(ValidationMessages.Required, "{PropertyName}"));
        //}

        //public static IRuleBuilderOptions<T, string> DecimalStringBetween<T>(this IRuleBuilder<T, string> ruleBuilder, decimal lowerBound, decimal upperBound)
        //{
        //    return ruleBuilder
        //        .Must(x => decimal.TryParse(x, out decimal result) && result >= lowerBound && result <= upperBound)
        //        .WithMessage(x => string.Format(ValidationMessages.Between, "{PropertyName}", lowerBound.ToString("N2"), upperBound.ToString("N2")));
        //}

        //public static IRuleBuilderOptions<T, string> DecimalStringNoGreaterThan<T>(this IRuleBuilder<T, string> ruleBuilder, decimal limit)
        //{
        //    return ruleBuilder
        //        .Must(x => decimal.TryParse(x, out decimal result) && result <= limit)
        //        .WithMessage(x => string.Format(ValidationMessages.NoGreaterThan, "{PropertyName}", limit.ToString("N0")));
        //}

        //public static IRuleBuilderOptions<T, int?> Required<T>(this IRuleBuilder<T, int?> ruleBuilder)
        //{
        //    return ruleBuilder
        //        .Must(x => x.HasValue)
        //        .WithMessage(x => string.Format(ValidationMessages.Required, "{PropertyName}"));
        //}

        //public static IRuleBuilderOptions<T, string> ValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
        //{
        //    // this is the same regex validation performed by the EmailAddress data annotation but with the timeout portion removed.
        //    var pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
        //    var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

        //    return ruleBuilder
        //        .Must(x => regex.Match(x).Length > 0)
        //        .WithMessage(x => string.Format(ValidationMessages.ValidEmail, "{PropertyName}"));
        //}
    }
}
