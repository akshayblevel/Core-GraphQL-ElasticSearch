using CoreGraphQLElasticSearch.Models;
using GraphQL.Types;

namespace CoreGraphQLElasticSearch.GraphQLModel
{
    public class EmployeeCertificationType : ObjectGraphType<Certification>
    {
        public EmployeeCertificationType()
        {
            Field(t => t.id);
            Field(t => t.employeeid);
            Field(t => t.title, nullable: true);
            Field(t => t.month, nullable: true);
            Field(t => t.year, nullable: true);
            Field(t => t.provider);
        }
    }

    
    public class EmployeeType : ObjectGraphType<Employee>
    {
        public EmployeeType()
        {
            Field(a => a.id);
            Field(a => a.name);
            Field(a => a.email);
            Field(a => a.mobile);
            Field(a => a.company);
            Field(a => a.address);
            Field(a => a.shortdescription);
            Field(a => a.longdescription);

            Field<EmployeeCertificationType>( "certifications",
                resolve: context => context.Source.certifications);
           
        }
    } 
}
