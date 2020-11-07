using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using API.Dto;
using API.Infrastructure;
using API.Model;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly MovieRamaContext _dbContext;
        private readonly IHashingService _hashingService;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly AuthenticationService _authService;

        public UsersController(MovieRamaContext dbContext, IHashingService hashingService, IJwtService jwtService, IRefreshTokenService refreshTokenService, AuthenticationService authService)
        {
            _dbContext = dbContext;
            _hashingService = hashingService;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("/api/user-authentication")]
        public async Task<IActionResult> Authenticate(AuthenticateUserRequest request)
        {
            var user = await _authService.Authenticate(request.Username, request.Password);

            if (user is null)
            {
                return Unauthorized();
            }

            var token = _jwtService.GenerateToken(user.Id, user.Username);

            var refreshTokenValue = _refreshTokenService.GenerateRefreshToken();

            var refreshToken = RefreshToken.Create(refreshTokenValue, user.Id);

            _dbContext.RefreshTokens.Add(refreshToken);

            await _dbContext.SaveChangesAsync();

            return Ok(new { user.Username, token, refreshToken = refreshToken.Token, RefreshTokenExpiresAt = refreshToken.ExpirationDate });

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user =  await _dbContext.Users.FindAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(new { user.Username });

        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            var hashedPassword = _hashingService.GetHashedValue(request.Password);
            var user = new User(request.Username, hashedPassword);
            _dbContext.Users.Add(user);

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = user.Id }, default);
        }
    }
}
