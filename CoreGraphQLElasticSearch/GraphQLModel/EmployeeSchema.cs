using GraphQL;
using GraphQL.Types;
using System;

namespace CoreGraphQLElasticSearch.GraphQLModel
{
    public class EmployeeSchema : Schema
    {
        public EmployeeSchema(IDependencyResolver resolver) : base(resolver)
        {
            try
            {
                Query = resolver.Resolve<EmployeeQuery>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }

}
