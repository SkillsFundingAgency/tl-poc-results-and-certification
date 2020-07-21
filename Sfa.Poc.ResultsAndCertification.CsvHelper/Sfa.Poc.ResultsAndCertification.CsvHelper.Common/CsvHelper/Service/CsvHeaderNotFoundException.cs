using System;
using System.Runtime.Serialization;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Service
{
    [Serializable]
    internal class CsvHeaderNotFoundException : Exception
    {
        public CsvHeaderNotFoundException(Exception ex)
        {
        }

        public CsvHeaderNotFoundException(string message) : base(message)
        {
        }

        public CsvHeaderNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CsvHeaderNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}