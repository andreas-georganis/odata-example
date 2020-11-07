using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Services
{
    public class JwtService : IJwtService
    {
        private readonly IOptionsMonitor<JwtSettings> _options;

        public JwtService(IOptionsMonitor<JwtSettings> options) 
            => _options = options;
        
        public string GenerateToken(int userId,string username)
        {
            var tokenSettings = _options.CurrentValue;

            var tokenHandler = new JwtSecurityTokenHandler();

            var secretInBytes = Encoding.ASCII.GetBytes(tokenSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = tokenSettings.Issuer,
                Audience = tokenSettings.Audience,
                Expires = DateTime.UtcNow.AddMinutes(tokenSettings.TokenExpirationThresholdInMinutes),
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.NameIdentifier, userId.ToString()) }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretInBytes), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateEncodedJwt(tokenDescriptor);

            return token;
        }

        public string ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_options.CurrentValue.Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                var username = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                return username;
            }
            catch (Exception)
            {

                return default;
            }
        }
    }

    public class JwtSettings
    {
        public string Secret { get; set; } 

        public string Issuer { get; set; } 

        public string Audience { get; set; }

        public int TokenExpirationThresholdInMinutes { get; set; } = (int)TimeSpan.FromMinutes(30).TotalMinutes;

    }
}
