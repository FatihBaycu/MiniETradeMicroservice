namespace MiniETradeMicroservice.Gateway.YARP.Models
{
    public class JWTTokenOptionModel
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int ExpireDuration { get; set; }
    }
}
