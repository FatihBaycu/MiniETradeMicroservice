namespace MiniETradeMicroservice.Products.WebAPI.Models
{
    public class ApiUrl
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public List<Url> Urls { get; set; }
    }
    public class Url
    {
        public string EndpointName { get; set; }
        public string EndpointUrl { get; set; }
    }
}
