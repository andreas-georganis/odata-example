using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace API.Services
{
    public class HashingService : IHashingService, IDisposable
    {
        const int NumberOfBytes = 256 / 8;
        const int SaltSize = 128 / 8;
        const int IterationCount = 1000;

        private readonly RandomNumberGenerator _rng;
        public HashingService()
        {
            _rng = RandomNumberGenerator.Create();
        }

        public void Dispose()
        {
            _rng.Dispose();
        }

        public string GetHashedValue(string plainTextValue)
        {
            byte[] salt = new byte[SaltSize];
            _rng.GetBytes(salt);

            var key = KeyDerivation.Pbkdf2(password: plainTextValue, salt: salt, prf: KeyDerivationPrf.HMACSHA1, iterationCount: IterationCount, numBytesRequested: NumberOfBytes);

            var hashedBytes = new byte[SaltSize + NumberOfBytes];
            Buffer.BlockCopy(salt, 0, hashedBytes, 0, SaltSize);
            Buffer.BlockCopy(key, 0, hashedBytes, SaltSize, NumberOfBytes);

            string hashed = Convert.ToBase64String(hashedBytes);

            return hashed;
        }

        public bool Verify(string hashedValue, string providedValue)
        {
            byte[] hashedValueBytes = Convert.FromBase64String(hashedValue);

            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(hashedValueBytes, 0, salt, 0, SaltSize);

            byte[] expectedHashedValue = new byte[NumberOfBytes];
            Buffer.BlockCopy(hashedValueBytes, SaltSize, expectedHashedValue, 0, NumberOfBytes);

            var actualHashedValue = KeyDerivation.Pbkdf2(password: providedValue, salt: salt, prf: KeyDerivationPrf.HMACSHA1, iterationCount: IterationCount, numBytesRequested: NumberOfBytes);

            return expectedHashedValue.SequenceEqual(actualHashedValue);
        }
    }
}
