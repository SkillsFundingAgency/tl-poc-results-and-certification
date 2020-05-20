using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Model;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRepository<TlPathway> _pathwayRepository;

        public RegistrationService(IRepository<TlPathway> pathwayRepository)
        {
            _pathwayRepository = pathwayRepository;
        }

        public IEnumerable<Tlevel> GetAllTLevelsByAoUkprn(long ukPrn)
        {
            // TODO: Select Tlevels from Database.
            return new List<Tlevel>();
        }
    }
}
