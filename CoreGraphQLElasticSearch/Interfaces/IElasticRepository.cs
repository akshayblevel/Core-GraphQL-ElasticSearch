using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreGraphQLElasticSearch
{
    public interface IElasticRepository<T> where T : class
    {
        Task<bool> DeleteIndex();
        Task<bool> AddIndex();
        Task<T> GetSingleOrDefault(Expression<Func<T, bool>> predicate);
        Task<bool> UpdateDocument(T value);
        Task<bool> AddDocuments(List<T> documents);
        Task<bool> AddDocument(T document);
        Task<List<T>> GetDocuments(List<long> offerKey);
    }
}
