using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MiniETradeMicroservice.Gateway.YARP.Entities;
using MiniETradeMicroservice.Gateway.YARP.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniETradeMicroservice.Gateway.YARP.Services
{
    public class JWTManager : IJWTService
    {
        private IOptions<JWTTokenOptionModel> _configuration;
        public JWTManager(IOptions<JWTTokenOptionModel> configuration)
        {
            _configuration = configuration;
        }
        public string CreateToken(User user)
        {
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _configuration.Value.Issuer,
                audience: _configuration.Value.Audience,
                claims: getClaims(user),
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(_configuration.Value.ExpireDuration),
                signingCredentials: getSigningCredentials()
            );
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(jwt);
        }
        
        #region PrivateMethods
        private List<Claim> getClaims(User user)
        {
            return new List<Claim>()
            {
                new Claim("UserId",user.Id.ToString()),
                new Claim("UserName",user.Username.ToString()),
            };
        }
        private SigningCredentials getSigningCredentials()
        {
            SymmetricSecurityKey synmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Value.SecretKey));
            return new SigningCredentials(
                key: synmetricSecurityKey,
                algorithm: SecurityAlgorithms.HmacSha512
                );
        } 
        #endregion
    }
}
