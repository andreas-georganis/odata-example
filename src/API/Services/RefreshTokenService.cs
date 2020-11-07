using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace API.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly RandomNumberGenerator _rng;
        public RefreshTokenService()
        {
            _rng = RandomNumberGenerator.Create();
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            _rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}
