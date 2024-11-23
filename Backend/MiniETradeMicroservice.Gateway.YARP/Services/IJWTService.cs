using MiniETradeMicroservice.Gateway.YARP.Entities;

namespace MiniETradeMicroservice.Gateway.YARP.Services
{
    public interface IJWTService
    {
        public string CreateToken(User user);
    }
}
