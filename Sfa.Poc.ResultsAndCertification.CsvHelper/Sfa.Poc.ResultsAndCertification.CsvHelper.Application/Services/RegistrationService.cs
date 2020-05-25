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
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Comparer;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Repositories;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRepository<TlPathway> _pathwayRepository;
        private readonly ResultsAndCertificationDbContext ctx;
        private readonly IRepository<TqRegistration> _tqRegistrationRepository;
        private readonly IRepository<TqSpecialismRegistration> _tqSpecialismRegistrationRepository;
        private readonly IRegistrationRepository _registrationRepository;

        public RegistrationService(IRepository<TlPathway> pathwayRepository, ResultsAndCertificationDbContext context,
            IRepository<TqRegistration> tqRegistrationRepository, IRepository<TqSpecialismRegistration> tqSpecialismRegistrationRepository,
            IRegistrationRepository registrationRepository)
        {
            _pathwayRepository = pathwayRepository;
            _tqRegistrationRepository = tqRegistrationRepository;
            _tqSpecialismRegistrationRepository = tqSpecialismRegistrationRepository;
            _registrationRepository = registrationRepository;
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
                                        (string.IsNullOrWhiteSpace(x.Specialism1) || t.TlSpecialismLarIds.Contains(x.Specialism1)) &&
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

            foreach (var r in results)
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

        public async Task CompareRegistrations()
        {
            var seedValue = 0;
            var entitiesToLoad = 10000;
            var ulns = new HashSet<long>();
            var registrations = new List<TqRegistration>();
            var dateTimeNow = DateTime.Now;
            Random random = new Random();
            for (int i = 1; i <= entitiesToLoad; i++)
            {
                ulns.Add(seedValue + i);

                var reg = new TqRegistration
                {
                    //Id = i,
                    UniqueLearnerNumber = seedValue + i,
                    Firstname = "Firstname " + (seedValue + i),
                    Lastname = "Lastname " + (seedValue + i),
                    DateofBirth = DateTime.UtcNow.AddDays(97),
                    TqProviderId = 1,
                    StartDate = dateTimeNow.Date.AddDays(-3),
                    Status = 2
                };

                if (i <= entitiesToLoad)
                {
                    reg.TqSpecialismRegistrations = new List<TqSpecialismRegistration>
                    {
                        new TqSpecialismRegistration
                        {
                            //TqRegistrationId = 1,
                            TlSpecialismId = 1,
                            Status = 1
                        }
                        //new TqSpecialismRegistration
                        //{
                        //    //TqRegistrationId = 1,
                        //    TlSpecialismId = 2,
                        //    Status = 1
                        //}
                    };
                }

                registrations.Add(reg);
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            watch.Start();

            var existingRegistrationsFromDb = await _tqRegistrationRepository
                                                    .GetManyAsync(x => x.Status == 1 && ulns.Contains(x.UniqueLearnerNumber),
                                                                  x => x.TqProvider, x => x.TqSpecialismRegistrations)
                                                    .ToListAsync();

            var comparer = new TqRegistrationEqualityComparer();
            var newRegistrations = registrations.Where(x => !existingRegistrationsFromDb.Any(e => e.UniqueLearnerNumber == x.UniqueLearnerNumber)).ToList();
            var matchedRegistrations = registrations.Where(x => existingRegistrationsFromDb.Any(e => e.UniqueLearnerNumber == x.UniqueLearnerNumber)).ToList();
            var sameOrDuplicateRegistrations = matchedRegistrations.Intersect(existingRegistrationsFromDb, comparer).ToList();

            var modifiedRegistrations = new List<TqRegistration>();

            if (matchedRegistrations.Count != sameOrDuplicateRegistrations.Count)
            {
                modifiedRegistrations = matchedRegistrations.Except(sameOrDuplicateRegistrations, comparer).ToList();

                //var sameOrDuplicateRegistrations = existingRegistrationsFromDb.Intersect(registrations, comparer).ToList();
                //var modifiedRegistrations = matchedRegistrations.Except(sameOrDuplicateRegistrations, comparer).ToList();

                var specialismComparer = new TqSpecialismRegistrationEqualityComparer();

                modifiedRegistrations.ForEach(mr =>
                {
                    var reg = existingRegistrationsFromDb.FirstOrDefault(x => x.UniqueLearnerNumber == mr.UniqueLearnerNumber);

                    if (reg != null)
                    {
                        mr.Id = reg.Id;

                        var specialismsInDb = reg.TqSpecialismRegistrations.Where(s => s.Status == 1);

                        // update TqRegistrationId
                        mr.TqSpecialismRegistrations.ToList().ForEach(sp => sp.TqRegistrationId = reg.Id);

                        //1.Check if this registration belongs to this AO. If not throw validation error -- TODO
                        //2.Check if provider has changed or not -- TODO need to check with business
                        //3.Check if specialism has changed or not, if changed then update status to 2 // withdraw and create new record

                        //1,1 -2,1
                        //3,1 2,1 - 1,1 2,1
                        //3,1 4,1 - 1,1 2,1
                        //3,1 4,1 - 1,1 2,1 4,1

                        // below commented line using EqualityComprarer
                        //var specialismsToAdd = mr.TqSpecialismRegistrations.Except(specialismsInDb, specialismComparer).ToList();
                        //var specialismsToUpdate = specialismsInDb.Except(mr.TqSpecialismRegistrations, specialismComparer).ToList();

                        var specialismsToAdd = mr.TqSpecialismRegistrations.Where(s => !specialismsInDb.Any(r => r.TlSpecialismId == s.TlSpecialismId && r.Status == 1)).ToList();
                        var specialismsToUpdate = specialismsInDb.Where(s => !mr.TqSpecialismRegistrations.Any(r => r.TlSpecialismId == s.TlSpecialismId && r.Status == 1)).ToList();

                        specialismsToUpdate.ForEach(s => s.Status = 2); // change the status to inactive or withdrawn

                        mr.TqSpecialismRegistrations.Clear();
                        mr.TqSpecialismRegistrations = specialismsToAdd.Concat(specialismsToUpdate).ToList();
                    }
                });
            }

            if (newRegistrations.Count > 0 || modifiedRegistrations.Count > 0)
            {
                var registrationsToSendToDB = newRegistrations.Concat(modifiedRegistrations).ToList();
                await _registrationRepository.BulkInsertOrUpdateRegistrations(registrationsToSendToDB);
                //await _tqRegistrationRepository.BulkInsertOrUpdateAsync(registrationsToSendToDB);
            }
            watch.Stop();
            var sec = watch.ElapsedMilliseconds;
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
