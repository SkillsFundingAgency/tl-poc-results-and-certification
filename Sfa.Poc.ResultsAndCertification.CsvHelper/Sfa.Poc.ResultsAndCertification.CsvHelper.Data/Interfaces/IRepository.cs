using Sfa.Poc.ResultsAndCertification.CsvHelper.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Poc.ResultsAndCertification.CsvHelper.Data.Interfaces
{
    public interface IRepository<T> where T : BaseEntity, new()
    {
        Task<int> CreateAsync(T entity);
        Task<int> CreateManyAsync(IList<T> entities);
        Task<int> UpdateAsync(T entity);
        Task<int> UpdateWithSpecifedColumnsOnlyAsync(T entity, params Expression<Func<T, object>>[] properties);
        Task<int> DeleteAsync(int id);
        Task<int> DeleteAsync(T entity);
        IQueryable<T> GetManyAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] navigationPropertyPath);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertyPath);
        Task<TDto> GetFirstOrDefaultAsync<TDto>(Expression<Func<T, bool>> predicate, Expression<Func<T, TDto>> selector, Expression<Func<T, object>> orderBy = null, bool asendingorder = true, params Expression<Func<T, object>>[] navigationPropertyPath);
        Task<T> GetSingleOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertyPath);
        Task<TDto> GetSingleOrDefaultAsync<TDto>(Expression<Func<T, bool>> predicate, Expression<Func<T, TDto>> selector, Expression<Func<T, object>> orderBy = null, bool asendingorder = true, params Expression<Func<T, object>>[] navigationPropertyPath);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);

        // Bulk Operations
        Task<IList<T>> BulkReadAsync(IList<T> entities, params Expression<Func<T, object>>[] whereClauseProperties);
        Task BulkInsertAsync(IList<T> entities);
        Task BulkUpdateAsync(IList<T> entities);
        //Task BulkInsertOrUpdateAsync(IList<T> entities); 
        Task<IList<T>> BulkInsertOrUpdateAsync(IList<T> entities, params Expression<Func<T, object>>[] updatePropertiesBy);
        //Task<IList<T>> BulkInsertOrUpdateAsync(IList<T> entities);
    }
}
