namespace CoreGraphQLElasticSearch.Models
{
    public class GraphRequest
    {
        public string Query { get; set; }

        public string OperationName { get; set; }

        public object Variables { get; set; }
    }
}
