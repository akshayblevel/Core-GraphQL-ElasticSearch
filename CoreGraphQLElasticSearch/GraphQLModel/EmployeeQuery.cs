using GraphQL.Types;
using System;
using System.Collections.Generic;

namespace CoreGraphQLElasticSearch.GraphQLModel
{
    public class EmployeeQuery : ObjectGraphType
    {
        public EmployeeQuery(IEmployeeRepository employeeRepository)
        {
            try
            { 
                Field<ListGraphType<EmployeeType>>(
                    "Employees",
                    arguments: new QueryArguments(new QueryArgument<NonNullGraphType<ListGraphType<IdGraphType>>>
                    { Name = "id" }),
                    resolve: context =>
                    {
                        var id = context.GetArgument<List<long>>("id");
                        return employeeRepository.GetEmployees(id);
                    }
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }

}
