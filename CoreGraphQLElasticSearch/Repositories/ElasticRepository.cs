using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Nest;

namespace CoreGraphQLElasticSearch.Repositories
{
    public class ElasticRepository<T> : IElasticRepository<T> where T : class
    {
        private readonly ElasticClient _esClient;
        private string _indexValue;
        private const string DefaultIndex = "default-index";

        public ElasticRepository()
        {
            _esClient = new ElasticClient(new Uri("http://localhost:9200/"));
            _indexValue = GetDescription();

            //DeleteIndex().GetAwaiter().GetResult();
            //AddIndex().GetAwaiter().GetResult();


        }

        private string GetDescription()
        {
            var descriptionAttributes = (DescriptionAttribute[])typeof(T).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (descriptionAttributes.Length == 0) return DefaultIndex;
            return descriptionAttributes[0].Description;
        }

        public async Task<bool> AddDocuments(List<T> documents)
        {
            var waitHandle = new CountdownEvent(1);

            var bulkAll = _esClient.BulkAll(documents, b =>
                         b.Index(_indexValue)
                         .BackOffRetries(2)
                         .BackOffTime("30s")
                         .RefreshOnCompleted(true)
                         .MaxDegreeOfParallelism(4)
                         .Size(1000));

            bulkAll.Subscribe(new BulkAllObserver(
                    onNext: (b) => { Console.Write("."); },
                    onError: (e) => { throw e; },
                    onCompleted: () => waitHandle.Signal()
                ));

            waitHandle.Wait();

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteIndex()
        {
            var result = await _esClient.DeleteByQueryAsync<T>(q => q.Index(_indexValue));

            return result.IsValid;
        }

        public async Task<List<T>> GetDocuments(Guid[] offerKey)
        {
            var searchResponse = await _esClient.SearchAsync<T>(s => s
                .Query(q => q.Bool(m => m.Filter(flter => flter.Ids(i => i.Name("id").Values(offerKey)))))
                .Index(Indices.Index(_indexValue))
                .Size(2000)

            );

            return await RemapDocument(searchResponse.Documents);
        }

        public async Task<List<T>> GetDocuments(List<long> offerKey)
        {
            try
            {
                var searchResponse = await _esClient.SearchAsync<T>(s => s
                    .Query(q => q.Bool(m => m.Filter(flter => flter.Ids(i => i.Name("id").Values(offerKey)))))
                    .Index(Indices.Index(_indexValue))
                    .Size(2000)
                );

                return await RemapDocument(searchResponse.Documents);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                var trace = e.Message;
                throw;
            }

        }

        public async Task<T> GetSingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            var searchResponse = await _esClient.SearchAsync<T>(s => s
                .Query(q => q.Bool(m => m.Filter(flter => flter.Match(mt => mt.Field(predicate))))).Index(Indices.Index(_indexValue))
            );

            var results = searchResponse.Documents;

            return results.FirstOrDefault();
        }

        public async Task<bool> UpdateDocument(T value)
        {
            var result = await _esClient.UpdateAsync(DocumentPath<T>
              .Id(value.GetType().GetProperty("Id") != null ? value.GetType().GetProperty("Id").GetValue(value, null).ToString() : Id.From(value)),
                   u => u
                  .Index(_indexValue)
                  .DocAsUpsert(true)
                  .Doc(value));

            return result.IsValid;
        }

        public async Task<bool> AddDocument(T document)
        {
            var result = await _esClient.IndexAsync(document, i => i.Index(_indexValue));

            return result.IsValid;
        }

        private async Task<List<T>> RemapDocument(IReadOnlyCollection<T> documents)
        {
            if (documents.Count > 0)
            {
                var results = documents.ToList();
                return await Task.FromResult(results);
            }
            else
            {
                return await Task.FromResult(documents.ToList());
            }
        }

        public async Task<bool> AddIndex()
        {
            var result = await _esClient.Indices.ExistsAsync(_indexValue);

            if (!result.Exists)
            {
                await _esClient.Indices.CreateAsync(_indexValue);
            }

            return result.Exists;
        }
    }
}
