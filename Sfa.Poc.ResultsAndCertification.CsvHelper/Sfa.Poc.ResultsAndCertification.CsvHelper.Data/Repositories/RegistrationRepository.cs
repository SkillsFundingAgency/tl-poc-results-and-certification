using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Repositories
{
    public class RegistrationRepository : GenericRepository<TqRegistration>, IRegistrationRepository
    {
        public RegistrationRepository(ILogger<RegistrationRepository> logger, ResultsAndCertificationDbContext dbContext) : base(logger, dbContext) { }

        public async Task<IList<TqRegistration>> BulkInsertOrUpdateRegistrations(List<TqRegistration> entities)
        {
            if (entities != null && entities.Count > 0)
            {
                var strategy = _dbContext.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var bulkConfig = new BulkConfig() { UseTempDB = true, SetOutputIdentity = true, CalculateStats = true };
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

                            if(specialismRegistrations.Count > 0)
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
    }
}
