using MiniETradeMicroservice.Gateway.YARP.Entities;

namespace MiniETradeMicroservice.Gateway.YARP.Dtos
{
    public class UserRegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public static implicit operator User(UserRegisterDto userRegisterDto)
        {
            return new User
            {
                Username = userRegisterDto.Username,
                Password = userRegisterDto.Password,
            };
        }
    }
}
