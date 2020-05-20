using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Model;
using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces
{
    public interface IRegistrationService
    {
       IEnumerable<Tlevel> GetAllTLevelsByAoUkprn(long ukPrn);
    }
}
