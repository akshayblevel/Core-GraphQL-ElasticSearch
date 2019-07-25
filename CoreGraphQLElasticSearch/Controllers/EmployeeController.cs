using System.Collections.Generic;
using System.Threading.Tasks;
using CoreGraphQLElasticSearch.Models;
using GraphQL.Client;
using GraphQL.Common.Request;
using Microsoft.AspNetCore.Mvc;

namespace CoreGraphQLElasticSearch.Controllers
{
    [Route("Employee")]
    public class EmployeeController : Controller
    {
        [HttpPost]
        public async Task<List<Employee>> Post([FromBody] GraphRequest req)
        {
            List<Employee> responseEmployees = new List<Employee>();
            using (GraphQLClient graphQLClient = new GraphQLClient("http://localhost:30528/graphql"))
            {
                var query = new GraphQLRequest
                {
                    Query = req.Query,
                    OperationName = req.OperationName,
                    Variables = req.Variables
                };
                var response = await graphQLClient.PostAsync(query);
                responseEmployees = response.GetDataFieldAs<List<Employee>>("Employees");
            }

            return responseEmployees;
        }
    }
}