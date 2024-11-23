using MiniETradeMicroservice.Gateway.YARP.Entities;

namespace MiniETradeMicroservice.Gateway.YARP.Dtos
{
    public class UserLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public static implicit operator User(UserLoginDto userRegisterDto)
        {
            return new User
            {
                Username = userRegisterDto.Username,
                Password = userRegisterDto.Password,
            };
        }
    }
}
