using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Model;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Data;
using Microsoft.EntityFrameworkCore;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Common.CsvHelper.Model;
using System.Threading.Tasks;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Comparer;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Repositories;
using Microsoft.EntityFrameworkCore.Internal;
using System.IO;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Application.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRepository<TlPathway> _pathwayRepository;
        private readonly ResultsAndCertificationDbContext ctx;
        private readonly IRepository<TqRegistration> _tqRegistrationRepository;
        private readonly IRepository<TqSpecialismRegistration> _tqSpecialismRegistrationRepository;
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IRepository<TqRegistrationProfile> _tqRegistrationProfileRepository;
        private readonly IRepository<TqRegistrationPathway> _tqRegistrationPathwayRepository;

        public RegistrationService(IRepository<TlPathway> pathwayRepository, ResultsAndCertificationDbContext context,
            IRepository<TqRegistration> tqRegistrationRepository, IRepository<TqSpecialismRegistration> tqSpecialismRegistrationRepository,
            IRegistrationRepository registrationRepository, IRepository<TqRegistrationProfile> tqRegistrationProfileRepository,
            IRepository<TqRegistrationPathway> tqRegistrationPathwayRepository)
        {
            _pathwayRepository = pathwayRepository;
            _tqRegistrationRepository = tqRegistrationRepository;
            _tqSpecialismRegistrationRepository = tqSpecialismRegistrationRepository;
            _registrationRepository = registrationRepository;
            _tqRegistrationProfileRepository = tqRegistrationProfileRepository;
            _tqRegistrationPathwayRepository = tqRegistrationPathwayRepository;
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

        public async Task<IEnumerable<Registration>> ValidateRegistrationTlevelsAsync(long ukprn, IEnumerable<Registration> regdata)
        {
            var aoProviderTlevels = await GetAllTLevelsByAoUkprnAsync(ukprn);

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
                                        (x.Specialisms?.Count() == 0 ||
                                        x.Specialisms.All(xs => t.TlSpecialismLarIds.Contains(xs))));

                //var isValidSpecialisms = true;
                if (!isValidSpecialisms)
                    AddValidationError(x, "Specialisms are not valid for T Level");

            });

            return regdata;
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
            var entitiesToLoad = 1;
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


        public async Task CompareAndProcessRegistrations()
        {
            var seedValue = 0;
            var entitiesToLoad = 10000;
            var ulns = new HashSet<int>();
            var registrations = new List<TqRegistrationProfile>();
            var dateTimeNow = DateTime.Now;
            Random random = new Random();
            for (int i = 1; i <= entitiesToLoad; i++)
            {
                if(i <= 10000)
                ulns.Add(seedValue + i);

                var reg = new TqRegistrationProfile
                {
                    //Id = i,
                    UniqueLearnerNumber = seedValue + i,
                    Firstname = "Firstname " + (seedValue + "XY" + i),
                    Lastname = "Lastname " + (seedValue + i),
                    DateofBirth = DateTime.Parse("17/01/1983")
                };

                reg.TqRegistrationPathways = new List<TqRegistrationPathway>
                    {
                        new TqRegistrationPathway
                        {
                            TqProviderId = 1,
                            StartDate = DateTime.Parse("01/06/2020"),
                            Status = 1,
                            TqRegistrationSpecialisms = new List<TqRegistrationSpecialism>
                            {
                                new TqRegistrationSpecialism
                                {
                                    TlSpecialismId = 17,
                                    StartDate = DateTime.Parse("21/07/2020"),
                                    Status = 1
                                }
                            }
                        }
                    };

                registrations.Add(reg);
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            watch.Start();

            //var tqRegistrationPathways = await _tqRegistrationPathwayRepository.GetManyAsync(p => ulns.Contains(p.TqRegistrationProfile.UniqueLearnerNumber),
            //    p => p.TqRegistrationProfile, p => p.TqRegistrationSpecialisms, p => p.TqProvider,
            //    p => p.TqProvider.TqAwardingOrganisation, p => p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton).ToListAsync();

            //watch.Stop();
            //var sec = watch.ElapsedMilliseconds;

            //watch.Reset();
            //watch.Start();

            var existingRegistrationsFromDb = await ctx.TqRegistrationProfile.Where(x => ulns.Contains(x.UniqueLearnerNumber))
                .Include(x => x.TqRegistrationPathways)
                    .ThenInclude(x => x.TqRegistrationSpecialisms)
                .Include(x => x.TqRegistrationPathways)
                .ThenInclude(x => x.TqProvider)
                        .ThenInclude(x => x.TqAwardingOrganisation)
                //.ThenInclude(x => x.TlAwardingOrganisaton)
                .ToListAsync();


            //watch.Stop();
            //var sec = watch.ElapsedMilliseconds;

            //var tqRegistrationProfiles = tqRegistrationPathways.SelectMany(x => x.TqRegistrationProfile, (x, prof) => new { x, prof}).ToList();

            //var existingRegistrationsFromDb = await _tqRegistrationProfileRepository
            //                                        .GetManyAsync(x => ulns.Contains(x.UniqueLearnerNumber),
            //                                                      x => x.TqRegistrationPathways)
            //                                        .ToListAsync();



            var comparer = new TqRegistrationProfileEqualityComparer();
            var ulnComparer = new TqRegistrationUlnEqualityComparer();
            //var newRegistrations = registrations.Where(x => !existingRegistrationsFromDb.Any(e => e.UniqueLearnerNumber == x.UniqueLearnerNumber)).ToList();
            //var matchedRegistrations = registrations.Where(x => existingRegistrationsFromDb.Any(e => e.UniqueLearnerNumber == x.UniqueLearnerNumber)).ToList();

            var newRegistrations = registrations.Except(existingRegistrationsFromDb, ulnComparer).ToList();
            var matchedRegistrations = registrations.Intersect(existingRegistrationsFromDb, ulnComparer).ToList();
            var sameOrDuplicateRegistrations = matchedRegistrations.Intersect(existingRegistrationsFromDb, comparer).ToList();

            var modifiedRegistrations = new List<TqRegistrationProfile>();

            if (matchedRegistrations.Count != sameOrDuplicateRegistrations.Count)
            {
                //modifiedRegistrations = matchedRegistrations.Where(r => !sameOrDuplicateRegistrations.Any(s => s.UniqueLearnerNumber == r.UniqueLearnerNumber)).ToList();
                modifiedRegistrations = matchedRegistrations.Except(sameOrDuplicateRegistrations, comparer).ToList();

                //var sameOrDuplicateRegistrations = existingRegistrationsFromDb.Intersect(registrations, comparer).ToList();
                //var modifiedRegistrations = matchedRegistrations.Except(sameOrDuplicateRegistrations, comparer).ToList();

                //var specialismComparer = new TqSpecialismRegistrationEqualityComparer();

                modifiedRegistrations.ForEach(mr =>
                {
                    if (mr.TqRegistrationPathways.Count > 1)
                        throw new ApplicationException();

                    //TODO: Need to check if there is an active registration for another AO, if so show error message and reject the file

                    var reg = existingRegistrationsFromDb.FirstOrDefault(x => x.UniqueLearnerNumber == mr.UniqueLearnerNumber);

                    if (reg != null)
                    {
                        mr.Id = reg.Id;
                        mr.TqRegistrationPathways.ToList().ForEach(p => p.TqRegistrationProfileId = reg.Id);

                        var pathwayRegistrationsInDb = reg.TqRegistrationPathways.Where(s => s.Status == 1);
                        var pathwaysToAdd = mr.TqRegistrationPathways.Where(s => !pathwayRegistrationsInDb.Any(r => r.TqProviderId == s.TqProviderId)).ToList();
                        
                        var pathwaysToUpdate = pathwayRegistrationsInDb.Where(s => mr.TqRegistrationPathways.Any(r => r.TqProviderId == s.TqProviderId)).ToList();

                        //pathwaysToUpdate
                        if (pathwaysToUpdate.Count > 0)
                        {
                            //var isProviderChanged = !pathwaysToUpdate.Any(x => mr.TqRegistrationPathways.Any(r => r.TqProvider.TlProviderId == x.TqProvider.TlProviderId));

                            //var isPathwayChanged = !pathwaysToUpdate.Any(x => mr.TqRegistrationPathways.Any(r => r.TqProvider.TqAwardingOrganisation.TlPathwayId == x.TqProvider.TqAwardingOrganisation.TlPathwayId));

                            //if (isPathwayChanged)
                            //{
                            //    var isAoChanged = !pathwaysToUpdate.Any(x => mr.TqRegistrationPathways.Any(r => r.TqProvider.TqAwardingOrganisation.TlAwardingOrganisatonId == x.TqProvider.TqAwardingOrganisation.TlAwardingOrganisatonId));
                                
                            //}

                            // change existing TqRegistrationPathway record status and related TqRegistrationSpecialism records status to "Changed"
                            if (pathwaysToAdd.Count > 0)
                            {
                                foreach (var pathway in pathwaysToUpdate)
                                {
                                    pathway.Status = 2; // update status to changed
                                    pathway.EndDate = DateTime.UtcNow;
                                    pathway.ModifiedBy = "LoggedIn User";
                                    pathway.ModifiedOn = DateTime.UtcNow;

                                    foreach (var specialism in pathway.TqRegistrationSpecialisms)
                                    {
                                        specialism.Status = 2; // update status to changed
                                        pathway.EndDate = DateTime.UtcNow;
                                        pathway.ModifiedBy = "LoggedIn User";
                                        pathway.ModifiedOn = DateTime.UtcNow;
                                    }
                                }
                            }
                            else
                            {
                                foreach(var importPathwayRecord in mr.TqRegistrationPathways)
                                {
                                    if (importPathwayRecord.TqRegistrationSpecialisms.Any())
                                    {
                                        var existingPathwayRecord = pathwaysToUpdate.FirstOrDefault(p => p.TqProviderId == importPathwayRecord.TqProviderId);

                                        if (existingPathwayRecord != null && existingPathwayRecord.TqRegistrationSpecialisms.Any())
                                        {
                                            var specialismsInDb = existingPathwayRecord.TqRegistrationSpecialisms.Where(s => s.Status == 1).ToList();

                                            // update TqRegistrationId
                                            //mr.TqSpecialismRegistrations.ToList().ForEach(sp => sp.TqRegistrationId = reg.Id);

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

                                            var specialismsToAdd = importPathwayRecord.TqRegistrationSpecialisms.Where(s => !specialismsInDb.Any(r => r.TlSpecialismId == s.TlSpecialismId)).ToList();
                                            var specialismsToUpdate = specialismsInDb.Where(s => !importPathwayRecord.TqRegistrationSpecialisms.Any(r => r.TlSpecialismId == s.TlSpecialismId)).ToList();

                                            specialismsToUpdate.ForEach(s =>
                                            {
                                                s.Status = 2; // change the status to inactive or withdrawn
                                                s.EndDate = DateTime.UtcNow;
                                                s.ModifiedBy = "LoggedIn User";
                                                s.ModifiedOn = DateTime.UtcNow;
                                            });

                                            specialismsToAdd.ForEach(s =>
                                            {
                                                s.TqRegistrationPathwayId = existingPathwayRecord.Id;
                                                s.Status = 1;
                                                s.StartDate = DateTime.UtcNow;
                                                s.CreatedBy = "LoggedIn User";
                                            });

                                            if (specialismsToAdd.Count > 0 || specialismsToUpdate.Count > 0)
                                            {
                                                existingPathwayRecord.TqRegistrationSpecialisms.Clear();
                                                existingPathwayRecord.TqRegistrationSpecialisms = specialismsToAdd.Concat(specialismsToUpdate).ToList();
                                            }
                                        }
                                        else if (existingPathwayRecord != null)
                                        {
                                            importPathwayRecord.TqRegistrationSpecialisms.ToList().ForEach(s =>
                                            {
                                                existingPathwayRecord.TqRegistrationSpecialisms.Add(new TqRegistrationSpecialism
                                                {
                                                    TqRegistrationPathwayId = existingPathwayRecord.Id,
                                                    TlSpecialismId = s.TlSpecialismId,
                                                    StartDate = s.StartDate,
                                                    Status = s.Status,
                                                    CreatedBy = s.CreatedBy
                                                });
                                            });
                                        }
                                    }
                                }                                
                            }
                        }

                        if(pathwaysToAdd.Count > 0)
                        {
                            foreach(var pathway in pathwaysToAdd)
                            {
                                pathway.TqRegistrationProfileId = reg.Id;
                            }
                        }

                        if (pathwaysToAdd.Count > 0 || pathwaysToUpdate.Count > 0)
                        {
                            mr.TqRegistrationPathways = pathwaysToAdd.Concat(pathwaysToUpdate).ToList();
                        }                        
                    }
                });
            }

            if (newRegistrations.Count > 0 || modifiedRegistrations.Count > 0)
            {
                var registrationsToSendToDB = newRegistrations.Concat(modifiedRegistrations).ToList();
                //await _registrationRepository.BulkInsertOrUpdateTqRegistrations(registrationsToSendToDB);
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
