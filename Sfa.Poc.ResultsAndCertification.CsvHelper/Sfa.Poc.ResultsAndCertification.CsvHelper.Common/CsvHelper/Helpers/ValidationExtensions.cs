using System;
using System.Globalization;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Helpers
{
    public static class ValidationExtensions
    {
        public static bool IsDateTime(this string value)
        {
            return DateTime.TryParse(value, out _);
        }

        public static bool IsDateTimeWithFormat(this string value)
        {
            return DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}
