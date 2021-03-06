﻿using FluentValidation;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Helpers;
using System;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.DataValidator
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> Required<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage(string.Format(ValidationMessages.Required, "{PropertyName}"));
        }

        public static IRuleBuilderOptions<T, string> MustBeNumberWithLength<T>(this IRuleBuilder<T, string> ruleBuilder, int length)
        {
            return ruleBuilder
                .Must(r => r.Length == length && int.TryParse(r, out int result))
                .WithMessage(string.Format(ValidationMessages.MustBeNumberWithLength, "{PropertyName}", length));
        }

        public static IRuleBuilderOptions<T, string> MaxStringLength<T>(this IRuleBuilder<T, string> ruleBuilder, int max)
        {
            return ruleBuilder
                .Must(r => r == null || r.Length <= max)
                .WithMessage(string.Format(ValidationMessages.StringLength, "{PropertyName}", max.ToString()));
        }

        public static IRuleBuilderOptions<T, string> ValidDate<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(y => y.IsDateTimeWithFormat())
                .WithMessage(string.Format(ValidationMessages.MustBeValidDate, "{PropertyName}"));
        }

        public static IRuleBuilderOptions<T, string> NotFutureDate<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(y => y.IsDateTimeWithFormat() && y.ParseStringToDateTime() < DateTime.Now)
                .WithMessage(string.Format(ValidationMessages.DateNotinFuture, "{PropertyName}"));
        }
    }
}
