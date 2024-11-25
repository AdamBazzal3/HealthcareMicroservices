using Microsoft.IdentityModel.Tokens;
using HealthcareIdentityAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthcareIdentityAPI.Utils
{
    public class Crypto
    {
        private readonly IConfiguration _configurationManager;
        public Crypto(IConfiguration configuration)
        {
            _configurationManager = configuration;
        }
        public string GenerateToken(User user)
        {
            var issuer = _configurationManager["Jwt:Issuer"];
            var audience = _configurationManager["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_configurationManager["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                        new Claim("Id", Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim("USER_ROLE", $"{user.Role}"),
                        new Claim(JwtRegisteredClaimNames.Jti,
                        Guid.NewGuid().ToString())
                    }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }
    }
}
