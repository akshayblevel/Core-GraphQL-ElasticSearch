using CoreGraphQLElasticSearch.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CoreGraphQLElasticSearch
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetEmployees(List<long> offerKeys);
    }

}
