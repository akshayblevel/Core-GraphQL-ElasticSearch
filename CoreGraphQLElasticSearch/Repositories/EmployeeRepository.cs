using CoreGraphQLElasticSearch.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CoreGraphQLElasticSearch.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IElasticRepository<Employee> _elasticRepository;
        public EmployeeRepository(IElasticRepository<Employee> elasticRepository)
        {
            _elasticRepository = elasticRepository;
        }

        public Task<List<Employee>> GetEmployees(List<long> offerKeys)
        {
            return _elasticRepository.GetDocuments(offerKeys);
        }
    }
}
