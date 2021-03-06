﻿using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Repositories
{
    public class RegistrationRepository : GenericRepository<TqRegistrationProfile>, IRegistrationRepository
    {
        public RegistrationRepository(ILogger<RegistrationRepository> logger, ResultsAndCertificationDbContext dbContext) : base(logger, dbContext) { }

        public async Task<IList<TqRegistration>> BulkInsertOrUpdateRegistrations(List<TqRegistration> entities)
        {
            if (entities != null && entities.Count > 0)
            {
                var strategy = _dbContext.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var bulkConfig = new BulkConfig() { UseTempDB = true, PreserveInsertOrder = true, SetOutputIdentity = true };
                            await _dbContext.BulkInsertOrUpdateAsync(entities, bulkConfig);

                            var specialismRegistrations = new List<TqSpecialismRegistration>();

                            foreach (var entity in entities)
                            {
                                if (entity.TqSpecialismRegistrations.Count > 0)
                                {
                                    foreach (var specialismRegistration in entity.TqSpecialismRegistrations)
                                    {
                                        specialismRegistration.TqRegistrationId = entity.Id;
                                    }
                                    specialismRegistrations.AddRange(entity.TqSpecialismRegistrations);
                                }
                            }

                            if (specialismRegistrations.Count > 0)
                            {
                                await _dbContext.BulkInsertOrUpdateAsync(specialismRegistrations, bulkConfig);
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message, ex.InnerException);
                            transaction.Rollback();
                            throw;
                        }
                    }
                });
            }
            return entities;
        }

        public async Task<IList<TqRegistrationProfile>> BulkInsertOrUpdateTqRegistrations(List<TqRegistrationProfile> entities)
        {
            if (entities != null && entities.Count > 0)
            {
                var strategy = _dbContext.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            //var bulkConfig = new BulkConfig() { UseTempDB = true, SetOutputIdentity = true, CalculateStats = true };
                            var bulkConfig = new BulkConfig() { UseTempDB = true, PreserveInsertOrder = true, SetOutputIdentity = true, BatchSize = 10000, BulkCopyTimeout = 0 };
                            await _dbContext.BulkInsertOrUpdateAsync(entities, bulkConfig);

                            var pathwayRegistrations = new List<TqRegistrationPathway>();

                            foreach (var entity in entities)
                            {
                                foreach (var pathwayRegistration in entity.TqRegistrationPathways)
                                {
                                    pathwayRegistration.TqRegistrationProfileId = entity.Id;
                                    pathwayRegistrations.Add(pathwayRegistration);
                                }
                            }

                            await _dbContext.BulkInsertOrUpdateAsync(pathwayRegistrations, bulkConfig);

                            var specialismRegistrations = new List<TqRegistrationSpecialism>();

                            foreach (var entity in pathwayRegistrations)
                            {
                                foreach (var specialismRegistration in entity.TqRegistrationSpecialisms)
                                {
                                    specialismRegistration.TqRegistrationPathwayId = entity.Id;
                                    specialismRegistrations.Add(specialismRegistration);
                                }
                            }

                            if (specialismRegistrations.Count > 0)
                            {
                                await _dbContext.BulkInsertOrUpdateAsync(specialismRegistrations, bulkConfig => { bulkConfig.UseTempDB = true; bulkConfig.BatchSize = 10000; bulkConfig.BulkCopyTimeout = 0; });
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message, ex.InnerException);
                            transaction.Rollback();
                            throw;
                        }
                    }
                });
            }
            return entities;
        }

        public async Task<bool> BulkInsertOrUpdateTqRegistrations(List<TqRegistrationProfile> profileEntities, List<TqRegistrationPathway> pathwayEntities, List<TqRegistrationSpecialism> specialismEntities)
        {
            var result = false;
            if ((profileEntities != null && profileEntities.Count > 0) || (pathwayEntities != null && pathwayEntities.Count > 0) || (specialismEntities != null && specialismEntities.Count > 0))
            {
                var strategy = _dbContext.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var bulkConfig = new BulkConfig() { UseTempDB = true, PreserveInsertOrder = true, SetOutputIdentity = true, BatchSize = 10000, BulkCopyTimeout = 0 };
                            var pathwayRegistrations = pathwayEntities ?? new List<TqRegistrationPathway>();

                            if (profileEntities != null && profileEntities.Count > 0)
                            {
                                await _dbContext.BulkInsertOrUpdateAsync(profileEntities, bulkConfig);

                                profileEntities.ToList().ForEach(profile =>
                                {
                                    profile.TqRegistrationPathways.ToList().ForEach(pathway =>
                                    {
                                        pathway.TqRegistrationProfileId = profile.Id;
                                        pathwayRegistrations.Add(pathway);
                                    });
                                });
                            }

                            var specialismRegistrations = specialismEntities ?? new List<TqRegistrationSpecialism>();

                            if (pathwayRegistrations.Count > 0)
                            {
                                await _dbContext.BulkInsertOrUpdateAsync(pathwayRegistrations, bulkConfig);

                                pathwayRegistrations.ForEach(pathway =>
                                {
                                    pathway.TqRegistrationSpecialisms.ToList().ForEach(specialism =>
                                    {
                                        specialism.TqRegistrationPathwayId = pathway.Id;
                                        specialismRegistrations.Add(specialism);
                                    });
                                });
                            }

                            if (specialismRegistrations.Count > 0)
                            {
                                await _dbContext.BulkInsertOrUpdateAsync(specialismRegistrations, bulkConfig => { bulkConfig.UseTempDB = true; bulkConfig.BatchSize = 10000; bulkConfig.BulkCopyTimeout = 0; });
                            }

                            transaction.Commit();
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message, ex.InnerException);
                            transaction.Rollback();
                            //throw;
                        }
                    }
                });
            }
            return result;
        }
    }
}