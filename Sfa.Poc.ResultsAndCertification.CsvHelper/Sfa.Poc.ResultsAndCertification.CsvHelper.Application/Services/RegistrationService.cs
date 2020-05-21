using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Model;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRepository<TlPathway> _pathwayRepository;
        private readonly IRepository<TqRegistration> _tqRegistrationRepository;

        public RegistrationService(IRepository<TlPathway> pathwayRepository, IRepository<TqRegistration> tqRegistrationRepository)
        {
            _pathwayRepository = pathwayRepository;
            _tqRegistrationRepository = tqRegistrationRepository;
        }

        public IEnumerable<Tlevel> GetAllTLevelsByAoUkprn(long ukPrn)
        {
            // TODO: Select Tlevels from Database.
            return new List<Tlevel>();
        }

        public async Task ReadRegistrations(IList<TqRegistration> registrations)
        {
            var entitiesToLoad = 100000;
            registrations = new List<TqRegistration>();
            var dateTimeNow = DateTime.Now;
            Random random = new Random();
            for (int i = 1; i <= entitiesToLoad; i++)
            {
                registrations.Add(new TqRegistration
                {
                    //Id = i,
                    UniqueLearnerNumber = 123456789, //random.Next(1, 100000),
                    Firstname = "Firstname " + i,
                    Lastname = "Lastname " + i,
                    //DateofBirth = DateTime.UtcNow.AddDays(-i),
                    TqProviderId = 1,
                    //StartDate = dateTimeNow.Date,
                    Status = 1
                });
            }
           var results = await _tqRegistrationRepository.BulkReadAsync(registrations, r => r.UniqueLearnerNumber, r => r.Firstname,
                r => r.Lastname, r => r.TqProviderId, r => r.Status);
        
        }

        public async Task ProcessRegistrations(IList<TqRegistration> registrations)
        {
            var entitiesToLoad = 100000;

            registrations = new List<TqRegistration>();
            var dateTimeNow = DateTime.Now;
            Random random = new Random();
            for (int i = 1; i <= entitiesToLoad; i++)
            {
                registrations.Add(new TqRegistration
                {
                    Id = i,
                    UniqueLearnerNumber = 123456789, //random.Next(1, 100000),
                    Firstname = "Firstname " + i,
                    Lastname = "Lastname " + i,
                    DateofBirth = DateTime.UtcNow.AddDays(-i),
                    TqProviderId = 1,
                    StartDate = dateTimeNow.Date,
                    Status = 1
                });
            }
            await _tqRegistrationRepository.BulkInsertOrUpdateAsync(registrations);
        }
    }
}
