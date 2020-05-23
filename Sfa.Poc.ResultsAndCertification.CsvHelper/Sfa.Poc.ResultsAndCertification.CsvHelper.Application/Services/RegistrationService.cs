using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Model;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Data;
using Microsoft.EntityFrameworkCore;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRepository<TlPathway> _pathwayRepository;
        private readonly ResultsAndCertificationDbContext ctx;
        private readonly IRepository<TqRegistration> _tqRegistrationRepository;
        private readonly IRepository<TqSpecialismRegistration> _tqSpecialismRegistrationRepository;

        public RegistrationService(IRepository<TlPathway> pathwayRepository, ResultsAndCertificationDbContext context,
            IRepository<TqRegistration> tqRegistrationRepository, IRepository<TqSpecialismRegistration> tqSpecialismRegistrationRepository)
        {
            _pathwayRepository = pathwayRepository;
            _tqRegistrationRepository = tqRegistrationRepository;
            _tqSpecialismRegistrationRepository = tqSpecialismRegistrationRepository;
            ctx = context;
        }

        public async Task<IEnumerable<Tlevel>> GetAllTLevelsByAoUkprnAsync(long ukprn)
        {
            var result = await ctx.TqProvider
                .Include(x => x.TlProvider)
                .Include(x => x.TqAwardingOrganisation)
                .ThenInclude(x => x.TlAwardingOrganisaton)
                .Include(x => x.TqAwardingOrganisation)
                .ThenInclude(x => x.TlPathway)
                .ThenInclude(x => x.TlSpecialisms)
                .Include(x => x.TlProvider)
                .Where(x => x.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == ukprn)
                .Select(x => new Tlevel
                {
                    ProviderUkprn = x.TlProvider.UkPrn,
                    TlPathwayId = x.TqAwardingOrganisation.TlPathway.Id,
                    PathwayLarId = x.TqAwardingOrganisation.TlPathway.LarId,
                    TlSpecialisms = x.TqAwardingOrganisation.TlPathway.TlSpecialisms.Select(s => s.Id).ToList(),
                    TlSpecialismLarIds = x.TqAwardingOrganisation.TlPathway.TlSpecialisms.Select(s => s.LarId).ToList()
                }).ToListAsync();

            return result;
        }

        public async Task<bool> SaveBulkRegistrationsAsync(IEnumerable<Registration> regdata, long ukprn)
        {
            // TOOD: 
            return await Task.Run(() => true);
        }

        public async Task<BulkRegistrationResponse> ValidateRegistrationTlevelsAsync(long ukprn, IEnumerable<Registration> regdata)
        {
            var aoProviderTlevels = await GetAllTLevelsByAoUkprnAsync(ukprn);

            var result = new BulkRegistrationResponse();
            regdata.ToList().ForEach(x =>
            {
                // Validation: AO not registered for the T level. 
                var isAoRegistered = aoProviderTlevels.Any(t => t.PathwayLarId == x.Core);
                if (!isAoRegistered)
                {
                    AddValidationError(x, "Ao not registered for T level or Invalid T level");
                    return;
                }

                // Validation: Provider not registered for the T level
                var isValidProvider = aoProviderTlevels.Any(t => t.ProviderUkprn == x.Ukprn && t.PathwayLarId == x.Core);
                if (!isValidProvider)
                {
                    AddValidationError(x, "Provider not registered for T level");
                    return;
                }
                    
                // Validation: Verify if valid specialisms are used.
                var isValidSpecialisms = aoProviderTlevels.Any(t => t.ProviderUkprn == x.Ukprn && 
                                        t.PathwayLarId == x.Core && 
                                        (string.IsNullOrWhiteSpace(x.Specialism1)|| t.TlSpecialismLarIds.Contains(x.Specialism1)) &&
                                        (string.IsNullOrWhiteSpace(x.Specialism2) || t.TlSpecialismLarIds.Contains(x.Specialism2)));

                if (!isValidSpecialisms)
                    AddValidationError(x, "Specialisms are not valid for T Level");

            });

            return result;
        }

        public async Task ReadRegistrations(IList<TqRegistration> registrations)
        {
            var seedValue = 0;
            var ulns = new HashSet<long>();
            //var ulns = new List<long>();
            var entitiesToLoad = 10000;
            registrations = new List<TqRegistration>();
            var dateTimeNow = DateTime.Now;
            Random random = new Random();
            for (int i = 1; i <= entitiesToLoad; i++)
            {
                ulns.Add(seedValue + i);
                registrations.Add(new TqRegistration
                {
                    //Id = i,
                    UniqueLearnerNumber = seedValue + i, //random.Next(1, 100000),
                    Firstname = "Firstname " + (seedValue + i),
                    Lastname = "Lastname " + (seedValue + i),
                    DateofBirth = DateTime.UtcNow.AddDays(-i),
                    TqProviderId = 1,
                    StartDate = dateTimeNow.Date,
                    Status = 1
                });
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            watch.Start();


            var list = await _tqRegistrationRepository.GetManyAsync(x => ulns.Contains(x.UniqueLearnerNumber),
            x => x.TqSpecialismRegistrations).AsNoTracking().ToListAsync();

            watch.Stop();
            var sec = watch.ElapsedMilliseconds;
            
            var results = await _tqRegistrationRepository.BulkReadAsync(registrations, r => r.UniqueLearnerNumber);

            var specialismRegistrations = new List<TqSpecialismRegistration>();

            foreach(var r in results)
            {
                specialismRegistrations.Add(new TqSpecialismRegistration
                {
                    TqRegistrationId = r.Id                    
                });
            }
            var specialismResults = await _tqSpecialismRegistrationRepository.BulkReadAsync(specialismRegistrations, r => r.TqRegistrationId);
        }

        public async Task ProcessRegistrations(IList<TqRegistration> registrations)
        {
            var entitiesToLoad = 10000000;

            registrations = new List<TqRegistration>();
            var dateTimeNow = DateTime.Now;
            Random random = new Random();
            for (int i = 1; i <= entitiesToLoad; i++)
            {
                registrations.Add(new TqRegistration
                {
                    //Id = i,
                    UniqueLearnerNumber = i, //random.Next(1, 100000),
                    Firstname = "Firstname " + i,
                    Lastname = "Lastname " + i,
                    DateofBirth = DateTime.UtcNow.AddDays(100),
                    TqProviderId = 1,
                    StartDate = dateTimeNow.Date,
                    Status = 1
                });
            }
            await _tqRegistrationRepository.BulkInsertOrUpdateAsync(registrations);
            //await _tqRegistrationRepository.BulkInsertOrUpdateAsync(registrations, r => r.UniqueLearnerNumber, r => r.Firstname, r => r.Lastname);
        }

        private void AddValidationError(Registration reg, string message)
        {
            reg.ValidationErrors.Add(new ValidationError
            {
                FieldName = "NA",
                FieldValue = "NA",
                RawRow = message,
                RowNum = reg.RowNum
            });
        }
    }
}
