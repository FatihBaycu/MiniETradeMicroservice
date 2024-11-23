namespace MiniETradeMicroservice.Gateway.YARP.Entities
{
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
