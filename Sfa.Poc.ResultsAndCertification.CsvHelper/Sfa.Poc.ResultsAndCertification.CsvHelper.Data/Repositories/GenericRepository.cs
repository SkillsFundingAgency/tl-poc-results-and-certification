﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Interfaces;
using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity, new()
    {
        protected readonly ILogger _logger;
        protected readonly ResultsAndCertificationDbContext _dbContext;

        public GenericRepository(ILogger<GenericRepository<T>> logger, ResultsAndCertificationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public virtual async Task<int> CreateAsync(T entity)
        {
            await _dbContext.AddAsync(entity);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }

            return entity.Id;
        }

        public virtual async Task<int> CreateManyAsync(IList<T> entities)
        {
            await _dbContext.AddRangeAsync(entities);

            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }

        public virtual async Task<int> UpdateAsync(T entity)
        {
            _dbContext.Update(entity);

            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }

        public virtual async Task<int> UpdateWithSpecifedColumnsOnlyAsync(T entity, params Expression<Func<T, object>>[] properties)
        {
            properties.ToList().ForEach(p =>
            {
                _dbContext.Entry(entity).Property(p).IsModified = true;
            });

            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }

        public virtual async Task<int> DeleteAsync(T entity)
        {
            _dbContext.Remove(entity);

            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }

        public virtual async Task<int> DeleteAsync(int id)
        {
            var entity = new T
            {
                Id = id
            };

            _dbContext.Attach(entity);
            _dbContext.Remove(entity);

            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }

        public virtual async Task DeleteManyAsync(IList<T> entities)
        {
            if (entities.Count == 0) return;

            _dbContext.RemoveRange(entities);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }

        public IQueryable<T> GetManyAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, null, true, navigationPropertyPath);
            return predicate != null ? queryable.Where(predicate) : queryable;
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, null, true, navigationPropertyPath);
            return await queryable.FirstOrDefaultAsync();
        }

        public async Task<TDto> GetFirstOrDefaultAsync<TDto>(Expression<Func<T, bool>> predicate, Expression<Func<T, TDto>> selector, Expression<Func<T, object>> orderBy, bool asendingorder = true, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, orderBy, asendingorder, navigationPropertyPath);
            return await queryable.Select(selector).FirstOrDefaultAsync();
        }

        public async Task<T> GetSingleOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, null, true, navigationPropertyPath);

            try
            {
                return await queryable.SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TDto> GetSingleOrDefaultAsync<TDto>(Expression<Func<T, bool>> predicate, Expression<Func<T, TDto>> selector, Expression<Func<T, object>> orderBy, bool asendingorder = true, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, orderBy, asendingorder, navigationPropertyPath);
            return await queryable.Select(selector).SingleOrDefaultAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate != null ? await _dbContext.Set<T>().CountAsync(predicate) :
                await _dbContext.Set<T>().CountAsync();
        }

        private IQueryable<T> GetQueryableWithIncludes(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy, bool asendingorder = true, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = _dbContext.Set<T>().AsQueryable();

            if (navigationPropertyPath != null && navigationPropertyPath.Any())
            {
                queryable = navigationPropertyPath.Aggregate(queryable, (current, navProp) => current.Include(navProp));
            }

            if (predicate != null)
                queryable = queryable.Where(predicate);

            if (orderBy != null)
                queryable = asendingorder ? queryable.OrderBy(orderBy) : queryable.OrderByDescending(orderBy);

            return queryable;
        }


        public virtual async Task BulkInsertAsync(IList<T> entities)
        {
            if (entities != null && entities.Count > 0)
            {
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _dbContext.BulkInsertAsync(entities, config => config.UseTempDB = true);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex.InnerException);
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public virtual async Task BulkUpdateAsync(IList<T> entities)
        {
            if (entities != null && entities.Count > 0)
            {
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _dbContext.BulkUpdateAsync(entities, config => config.UseTempDB = true);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex.InnerException);
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public virtual async Task<IList<T>> BulkInsertOrUpdateAsync(IList<T> entities, params Expression<Func<T, object>>[] updatePropertiesBy)
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

                //using (var transaction = _dbContext.Database.BeginTransaction())
                //{
                //    try
                //    {
                //        var bulkConfig = new BulkConfig() { UseTempDB = true, UseOnlyDataTable = true, SetOutputIdentity = true, CalculateStats = true };
                //        await _dbContext.BulkInsertOrUpdateAsync(entities, bulkConfig);
                //        transaction.Commit();
                //    }
                //    catch (Exception ex)
                //    {
                //        _logger.LogError(ex.Message, ex.InnerException);
                //        transaction.Rollback();
                //        throw;
                //    }
                //}

                //try
                //{
                //    var bulkConfig = new BulkConfig() { SetOutputIdentity = true, CalculateStats = true };

                //    if (updatePropertiesBy != null && updatePropertiesBy.Length > 0)
                //    {
                //        bulkConfig.UpdateByProperties = GetMemberNames(updatePropertiesBy);
                //    }                   

                //    await _dbContext.BulkInsertOrUpdateAsync(entities);
                //}
                //catch (Exception ex)
                //{
                //    _logger.LogError(ex.Message, ex.InnerException);
                //    throw;
                //}
            }
            return entities;
        }

        public virtual async Task<IList<T>> BulkReadAsync(IList<T> entities, params Expression<Func<T, object>>[] whereClauseProperties)
        {
            if (entities != null && entities.Count > 0)
            {
                try
                {
                    var properties = GetMemberNames(whereClauseProperties);
                    await _dbContext.BulkReadAsync(entities, bulkConfig => { bulkConfig.UpdateByProperties = properties; bulkConfig.TrackingEntities = true; });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex.InnerException);
                    throw;
                }
            }
            return entities;
        }

        private List<string> GetMemberNames(Expression<Func<T, object>>[] properties)
        {
            if (properties == null) return null;
            return properties.Select(p => { return (p.Body as MemberExpression ?? ((UnaryExpression)p.Body).Operand as MemberExpression).Member.Name; }).ToList();
        }
    }
}
