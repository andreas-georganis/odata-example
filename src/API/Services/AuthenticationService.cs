using API.Infrastructure;
using API.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class AuthenticationService
    {
        private readonly MovieRamaContext _dbContext;
        private readonly IHashingService _hashingService;

        public AuthenticationService(MovieRamaContext dbContext, IHashingService hashingService)
        {
            _dbContext = dbContext;
            _hashingService = hashingService;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

            var isValid = _hashingService.Verify(user.Password, password);

            if (isValid)
            {
                return user;
            }

            return default;
        }
    }
}
