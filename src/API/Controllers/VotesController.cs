using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dto;
using API.Infrastructure;
using API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VotesController : ControllerBase
    {
        private readonly MovieRamaContext _dbContext;

        public VotesController(MovieRamaContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPut]
        public async Task<IActionResult> Create(VoteDto request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = await _dbContext.Users.FindAsync(userId);

            user.Vote(request.MovieId, request.Type);

            await _dbContext.SaveChangesAsync();

            return Created(string.Empty, default);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Change(int id, VoteDto request)
        {
            var vote = await _dbContext.Votes.FindAsync(id);

            if (vote is null)
            {
                return NotFound();
            }

            vote.SetVoteType(request.Type);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Retract(int id)
        {
            var vote = await _dbContext.Votes.FindAsync(id);

            if (vote is null)
            {
                return NotFound();
            }

            _dbContext.Votes.Remove(vote);

            return NoContent();
        }
    }
}
