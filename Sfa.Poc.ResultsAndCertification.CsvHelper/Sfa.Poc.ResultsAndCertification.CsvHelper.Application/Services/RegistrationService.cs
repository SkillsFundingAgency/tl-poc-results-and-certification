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
using Microsoft.EntityFrameworkCore.Internal;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Models.BulkUpload;

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

        public async Task<IEnumerable<CoreAndSpecialisms>> GetAllTLevelsByAoUkprnAsync(long ukprn)
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
                .Select(x => new CoreAndSpecialisms
                {
                    ProviderUkprn = x.TlProvider.UkPrn,
                    TlPathwayId = x.TqAwardingOrganisation.TlPathway.Id,
                    PathwayLarId = x.TqAwardingOrganisation.TlPathway.LarId,
                    TqProviderId = x.Id,
                    TlProviderId = x.TlProviderId,
                    TqAwardingOrganisationId = x.TqAwardingOrganisationId,
                    TlAwardingOrganisatonId = x.TqAwardingOrganisation.TlAwardingOrganisatonId,
                    TlSpecialisms = x.TqAwardingOrganisation.TlPathway.TlSpecialisms.Select(s => s.Id).ToList(),
                    TlSpecialismLarIds = x.TqAwardingOrganisation.TlPathway.TlSpecialisms.Select(s => new KeyValuePair<int, string>(s.Id, s.LarId)).ToList()
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

            // Uln Duplicate
            var duplicateRegistrations = regdata
                .GroupBy(x => x.Uln)
               .Where(g => g.Count() > 1)
               .Select(y => y)
               .ToList();

            duplicateRegistrations.ForEach(x =>
            {
                x.ToList().ForEach(s => s.AddStage3Error(s.RowNum, "Uln duplicated"));
            });

            // Below are other than duplicated
            regdata.Where(x => x.IsValid)
                .ToList().ForEach(x =>
            {
                // Validation: AO not registered for the T level. 
                var isAoRegistered = aoProviderTlevels.Any(t => t.ProviderUkprn == x.Ukprn);
                if (!isAoRegistered)
                {
                    x.AddStage3Error(x.RowNum, "Provider not registered for AO.");
                    return;
                }

                // Validation: Provider not registered for the T level
                var isValidProvider = aoProviderTlevels.Any(t => t.ProviderUkprn == x.Ukprn && t.PathwayLarId == x.Core);
                if (!isValidProvider)
                {
                    x.AddStage3Error(x.RowNum, "Core is not registered with Provider.");
                    return;
                }

                //// Validation: Verify if valid specialisms are used.
                //var isValidSpecialisms = aoProviderTlevels.Any(t => t.ProviderUkprn == x.Ukprn &&
                //                        t.PathwayLarId == x.Core &&
                //                        (x.Specialisms?.Count() == 0 ||
                //                        x.Specialisms.All(xs => t.TlSpecialismLarIds.Select(splval => splval.Value).Contains(xs))));

                //if (!isValidSpecialisms)
                //{
                //    x.AddStage3Error(x.RowNum, "Specialisms are not valid for T Level");
                //    return;
                //}

                // Find a Tlevel record to assign fields. 
                var core = aoProviderTlevels.FirstOrDefault(ao => ao.PathwayLarId == x.Core && ao.ProviderUkprn == x.Ukprn);

                // New subset req.(Gurmukh)
                if (x.Specialisms.Count() > 0)
                {
                    var coreSpecialisms = core.TlSpecialismLarIds.Select(x => x.Value);
                    var invalidSpecialisms = x.Specialisms.Except(coreSpecialisms);

                    if (invalidSpecialisms.Count() > 0)
                    {
                        if (x.Specialisms.Count() == invalidSpecialisms.Count()) // i.e. all are invalid
                        {
                            x.AddStage3Error(x.RowNum, "Specialism not valid with core.");
                            return;
                        }

                        // partly invalid
                        var commaSpecialisms = string.Join(",", invalidSpecialisms);
                        x.AddStage3Error(x.RowNum, $"Incorrect rules of combination for Specialisms: {commaSpecialisms}" );
                        return;
                    }
                }
                
                if (core != null)
                {
                    x.TqProviderId = core.TqProviderId;
                    x.TqAwardingOrganisationId = core.TqAwardingOrganisationId;
                    x.TlSpecialismLarIds = core.TlSpecialismLarIds;
                    x.TlAwardingOrganisatonId = core.TlAwardingOrganisatonId;
                    x.TlProviderId = core.TlProviderId;
                }
                else
                {
                    // Todo: log and proceed.
                    x.AddStage3Error(x.RowNum, $"Core not found in the system. Core: {x.Core}, Ukprn: {x.Ukprn}");
                    return;
                }
            });

            return regdata;
        }

        public IEnumerable<TqRegistrationProfile> TransformRegistrationModel(IList<Registration> stageTwoResponse, string performedBy)
        {
            var learnerPathways = new List<TqRegistrationProfile>();
            stageTwoResponse.ToList().ForEach(x =>
            {
                learnerPathways.Add(new TqRegistrationProfile
                {
                    UniqueLearnerNumber = x.Uln,
                    Firstname = x.FirstName,
                    Lastname = x.LastName,
                    DateofBirth = x.DateOfBirth,
                    CreatedBy = performedBy,
                    CreatedOn = DateTime.UtcNow,

                    TqRegistrationPathways = new List<TqRegistrationPathway>
                    {
                        new TqRegistrationPathway
                        {
                            TqProviderId = x.TqProviderId,
                            StartDate = x.StartDate,
                            TqRegistrationSpecialisms = MapSpecialisms(x, performedBy),
                            TqProvider = new TqProvider
                            {
                                TqAwardingOrganisationId = x.TqAwardingOrganisationId,
                                TlProviderId = x.TlProviderId,
                                TqAwardingOrganisation = new TqAwardingOrganisation
                                {
                                    TlAwardingOrganisatonId = x.TlAwardingOrganisatonId,
                                    TlPathwayId = x.TlPathwayId,
                                }
                            },

                            Status = 1, // Todo: Enum statues.
                            CreatedBy = performedBy,
                            CreatedOn = DateTime.UtcNow
                        }
                    }
                }); ;
            });

            return learnerPathways;
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


        public async Task<BulkUploadResponse> CompareAndProcessRegistrations(IList<TqRegistrationProfile> registrations)
        {
            var result = new BulkUploadResponse();
            var ulns = new HashSet<int>();

            //var seedValue = 0;
            //var entitiesToLoad = 10000;
            //var ulns = new HashSet<int>();
            //var registrations = new List<TqRegistrationProfile>();
            //var dateTimeNow = DateTime.Now;
            //Random random = new Random();
            //for (int i = 1; i <= entitiesToLoad; i++)
            //{
            //    if (i <= 10000)
            //        ulns.Add(seedValue + i);

            //    var reg = new TqRegistrationProfile
            //    {
            //        //Id = i,
            //        UniqueLearnerNumber = seedValue + i,
            //        Firstname = "Firstname " + (seedValue + "XY" + i),
            //        Lastname = "Lastname " + (seedValue + i),
            //        DateofBirth = DateTime.Parse("17/01/1983")
            //    };

            //    reg.TqRegistrationPathways = new List<TqRegistrationPathway>
            //        {
            //            new TqRegistrationPathway
            //            {
            //                TqProviderId = 1,
            //                StartDate = DateTime.Parse("01/06/2020"),
            //                Status = 1,
            //                TqProvider = new TqProvider { TqAwardingOrganisationId = 1, TlProviderId = 1,  TqAwardingOrganisation = new TqAwardingOrganisation { TlAwardingOrganisatonId = 1, TlPathwayId = 1 } },
            //                //TqProvider = new TqProvider { TqAwardingOrganisationId = 1, TlProviderId = 2,  TqAwardingOrganisation = new TqAwardingOrganisation { TlAwardingOrganisatonId = 3, TlPathwayId = 5 } },
            //                TqRegistrationSpecialisms = new List<TqRegistrationSpecialism>
            //                {
            //                    new TqRegistrationSpecialism
            //                    {
            //                        TlSpecialismId = 17,
            //                        StartDate = DateTime.Parse("21/07/2020"),
            //                        Status = 1
            //                    }
            //                }
            //            }
            //        };

            //    registrations.Add(reg);
            //}

            var watch = System.Diagnostics.Stopwatch.StartNew();
            watch.Start();

            var existingRegistrationsFromDb = await ctx.TqRegistrationProfile.Where(x => ulns.Contains(x.UniqueLearnerNumber))
                .Include(x => x.TqRegistrationPathways)
                    .ThenInclude(x => x.TqRegistrationSpecialisms)
                .Include(x => x.TqRegistrationPathways)
                    .ThenInclude(x => x.TqProvider)
                        .ThenInclude(x => x.TqAwardingOrganisation)
                //.ThenInclude(x => x.TlAwardingOrganisaton)
                .ToListAsync();


            watch.Stop();
            var sec1 = watch.ElapsedMilliseconds;

            watch.Restart();

            var modifiedRegistrations = new List<TqRegistrationProfile>();
            var modifiedRegistrationsToIgnore = new List<TqRegistrationProfile>();
            var modifiedPathwayRecords = new List<TqRegistrationPathway>();
            var modifiedSpecialismRecords = new List<TqRegistrationSpecialism>();

            var ulnComparer = new TqRegistrationUlnEqualityComparer();
            var comparer = new TqRegistrationRecordEqualityComparer();

            var newRegistrations = registrations.Except(existingRegistrationsFromDb, ulnComparer).ToList();
            var matchedRegistrations = registrations.Intersect(existingRegistrationsFromDb, ulnComparer).ToList();
            var sameOrDuplicateRegistrations = matchedRegistrations.Intersect(existingRegistrationsFromDb, comparer).ToList();

            if (matchedRegistrations.Count != sameOrDuplicateRegistrations.Count)
            {
                modifiedRegistrations = matchedRegistrations.Except(sameOrDuplicateRegistrations, comparer).ToList();

                var tqRegistrationProfileComparer = new TqRegistrationProfileEqualityComparer();
                var tqRegistrationPathwayComparer = new TqRegistrationPathwayEqualityComparer();
                var tqRegistrationSpecialismComparer = new TqRegistrationSpecialismEqualityComparer();

                modifiedRegistrations.ForEach(modifiedRegistration =>
                {
                    var existingRegistration = existingRegistrationsFromDb.FirstOrDefault(existingRegistration => existingRegistration.UniqueLearnerNumber == modifiedRegistration.UniqueLearnerNumber);

                    if (existingRegistration != null)
                    {
                        var hasBothPathwayAndSpecialismsRecordsChanged = false;
                        var hasOnlySpecialismsRecordChanged = false;
                        var hasTqRegistrationProfileRecordChanged = !tqRegistrationProfileComparer.Equals(modifiedRegistration, existingRegistration);

                        modifiedRegistration.Id = existingRegistration.Id;
                        modifiedRegistration.TqRegistrationPathways.ToList().ForEach(p => p.TqRegistrationProfileId = existingRegistration.Id);

                        var pathwayRegistrationsInDb = existingRegistration.TqRegistrationPathways.Where(s => s.Status == 1).ToList();
                        var pathwaysToAdd = modifiedRegistration.TqRegistrationPathways.Where(s => !pathwayRegistrationsInDb.Any(r => r.TqProviderId == s.TqProviderId)).ToList();
                        var pathwaysToUpdate = pathwaysToAdd.Count > 0 ? pathwayRegistrationsInDb : pathwayRegistrationsInDb.Where(s => modifiedRegistration.TqRegistrationPathways.Any(r => r.TqProviderId == s.TqProviderId)).ToList();

                        if (pathwaysToUpdate.Count > 0)
                        {
                            var hasProviderChanged = !pathwaysToUpdate.Any(x => modifiedRegistration.TqRegistrationPathways.Any(r => r.TqProvider.TlProviderId == x.TqProvider.TlProviderId));
                            var hasPathwayChanged = !pathwaysToUpdate.Any(x => modifiedRegistration.TqRegistrationPathways.Any(r => r.TqProvider.TqAwardingOrganisation.TlPathwayId == x.TqProvider.TqAwardingOrganisation.TlPathwayId));

                            if (hasPathwayChanged)
                            {
                                //TODO: Need to check if there is an active registration for another AO, if so show error message and reject the file
                                var hasAoChanged = !pathwaysToUpdate.Any(x => modifiedRegistration.TqRegistrationPathways.Any(r => r.TqProvider.TqAwardingOrganisation.TlAwardingOrganisatonId == x.TqProvider.TqAwardingOrganisation.TlAwardingOrganisatonId));

                                if (hasAoChanged)
                                {
                                    result.BulkUploadErrors.Add(new BulkUploadError
                                    {
                                        FieldName = "Uln",
                                        FieldValue = modifiedRegistration.UniqueLearnerNumber.ToString(),
                                        ErrorMessage = "There is active registration with another Awarding Organisation"
                                    });
                                }
                            }

                            if (!result.HasAnyErrors)
                            {
                                // change existing TqRegistrationPathway record status and related TqRegistrationSpecialism records status to "Changed"
                                if (pathwaysToAdd.Count > 0)
                                {
                                    pathwaysToUpdate.ForEach(pathwayToUpdate =>
                                    {
                                        pathwayToUpdate.Status = 2; // update status to changed
                                        pathwayToUpdate.EndDate = DateTime.UtcNow;
                                        pathwayToUpdate.ModifiedBy = "LoggedIn User";
                                        pathwayToUpdate.ModifiedOn = DateTime.UtcNow;

                                        pathwayToUpdate.TqRegistrationSpecialisms.Where(s => s.Status == 1).ToList().ForEach(specialismToUpdate =>
                                        {
                                            specialismToUpdate.Status = 2; // update status to changed
                                            specialismToUpdate.EndDate = DateTime.UtcNow;
                                            specialismToUpdate.ModifiedBy = "LoggedIn User";
                                            specialismToUpdate.ModifiedOn = DateTime.UtcNow;
                                        });
                                    });
                                    hasBothPathwayAndSpecialismsRecordsChanged = true;
                                }
                                else
                                {
                                    modifiedRegistration.TqRegistrationPathways.ToList().ForEach(importPathwayRecord =>
                                    {
                                        var existingPathwayRecord = pathwaysToUpdate.FirstOrDefault(p => p.TqProviderId == importPathwayRecord.TqProviderId);
                                        if (existingPathwayRecord != null && existingPathwayRecord.TqRegistrationSpecialisms.Any())
                                        {
                                            var existingSpecialisms = existingPathwayRecord.TqRegistrationSpecialisms.Where(s => s.Status == 1).ToList();

                                            //1,1 - 2,1
                                            //3,1 2,1 - 1,1 2,1
                                            //3,1 4,1 - 1,1 2,1
                                            //3,1 4,1 - 1,1 2,1 4,1

                                            // below commented line using EqualityComprarer
                                            //var specialismsToAdd = mr.TqSpecialismRegistrations.Except(specialismsInDb, specialismComparer).ToList();
                                            //var specialismsToUpdate = specialismsInDb.Except(mr.TqSpecialismRegistrations, specialismComparer).ToList();

                                            var specialismsToAdd = importPathwayRecord.TqRegistrationSpecialisms.Where(s => !existingSpecialisms.Any(r => r.TlSpecialismId == s.TlSpecialismId)).ToList();
                                            var specialismsToUpdate = existingSpecialisms.Where(s => !importPathwayRecord.TqRegistrationSpecialisms.Any(r => r.TlSpecialismId == s.TlSpecialismId)).ToList();

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
                                                hasOnlySpecialismsRecordChanged = true;
                                                existingPathwayRecord.TqRegistrationSpecialisms.Clear();
                                                existingPathwayRecord.TqRegistrationSpecialisms = specialismsToAdd.Concat(specialismsToUpdate).ToList();
                                            }
                                        }
                                        else if (existingPathwayRecord != null && importPathwayRecord.TqRegistrationSpecialisms.Any())
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
                                            hasOnlySpecialismsRecordChanged = true;
                                        }
                                    });
                                }
                            }
                        }

                        if (!result.HasAnyErrors)
                        {
                            if (hasTqRegistrationProfileRecordChanged && hasBothPathwayAndSpecialismsRecordsChanged)
                            {
                                modifiedRegistration.TqRegistrationPathways = pathwaysToAdd.Concat(pathwaysToUpdate).ToList();
                            }
                            else if (hasTqRegistrationProfileRecordChanged && !hasBothPathwayAndSpecialismsRecordsChanged && hasOnlySpecialismsRecordChanged)
                            {
                                pathwaysToUpdate.ForEach(p => { modifiedSpecialismRecords.AddRange(p.TqRegistrationSpecialisms); });
                                modifiedRegistration.TqRegistrationPathways.Clear();
                            }
                            else if (hasTqRegistrationProfileRecordChanged && !hasBothPathwayAndSpecialismsRecordsChanged && !hasOnlySpecialismsRecordChanged)
                            {
                                modifiedRegistration.TqRegistrationPathways.Clear();
                            }
                            else if (hasBothPathwayAndSpecialismsRecordsChanged && !hasOnlySpecialismsRecordChanged)
                            {
                                modifiedPathwayRecords.AddRange(pathwaysToAdd.Concat(pathwaysToUpdate));
                                modifiedRegistrationsToIgnore.Add(modifiedRegistration);
                            }
                            else if (!hasBothPathwayAndSpecialismsRecordsChanged && hasOnlySpecialismsRecordChanged)
                            {
                                pathwaysToUpdate.ForEach(p => { modifiedSpecialismRecords.AddRange(p.TqRegistrationSpecialisms); });
                                modifiedRegistrationsToIgnore.Add(modifiedRegistration);
                            }
                        }
                    }
                });
            }

            if (!result.HasAnyErrors && (newRegistrations.Count > 0 || modifiedRegistrations.Count > 0))
            {
                var registrationsToSendToDB = newRegistrations.Concat(modifiedRegistrations.Except(modifiedRegistrationsToIgnore, ulnComparer)).ToList();
                //result.IsSuccess = await _registrationRepository.BulkInsertOrUpdateTqRegistrations(registrationsToSendToDB, modifiedPathwayRecords, modifiedSpecialismRecords);
                result.BulkUploadStats = new BulkUploadStats
                {
                    NewRecordsCount = newRegistrations.Count,
                    UpdatedRecordsCount = modifiedRegistrations.Count,
                    DuplicateRecordsCount = sameOrDuplicateRegistrations.Count
                };
            }

            watch.Stop();
            var sec = watch.ElapsedMilliseconds;

            return result;
        }

        private static List<TqRegistrationSpecialism> MapSpecialisms(Registration reg, string performedBy)
        {
            var regSpecialisms = new List<TqRegistrationSpecialism>();

            return reg.TlSpecialismLarIds.Select(x => new TqRegistrationSpecialism
            {
                StartDate = DateTime.UtcNow,
                TlSpecialismId = x.Key,
                Status = 1,                  // Todo: enum
                CreatedBy = performedBy,
                CreatedOn = DateTime.UtcNow,

            }).ToList();
        }
    }
}
