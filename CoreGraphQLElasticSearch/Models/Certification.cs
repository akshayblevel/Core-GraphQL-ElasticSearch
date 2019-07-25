namespace CoreGraphQLElasticSearch.Models
{
    public partial class Certification
    {
        public long id { get; set; }
        public long employeeid { get; set; }
        public string title { get; set; }
        public int? month { get; set; }
        public int? year { get; set; }
        public string provider { get; set; }
    }

}
