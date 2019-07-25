using System.Collections.Generic;
using System.ComponentModel;

namespace CoreGraphQLElasticSearch.Models
{
    [Description("employee")]
    public partial class Employee
    {
        public long id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string company { get; set; }
        public string address { get; set; }
        public string shortdescription { get; set; }
        public string longdescription { get; set; }

        public List<Certification> certifications { get; set; }
    }

}
