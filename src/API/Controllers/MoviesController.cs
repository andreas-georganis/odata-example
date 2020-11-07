using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dto;
using API.Infrastructure;
using API.Infrastructure.Attributes;
using API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.OData.Formatter.Value;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    [Authorize]
    [ValidateModelState]
    public class MoviesController : ODataController
    {
        private readonly MovieRamaContext _dbContext;

        public MoviesController(MovieRamaContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [EnableQuery]
        [AllowAnonymous]
        public ActionResult<IQueryable> Get()
        {
            var movies = _dbContext.Movies
                .Select(m => new MovieDto 
                { 
                    Id = m.Id, 
                    Title = m.Title, 
                    Description = m.Description, 
                    PublicationDate = m.PublicationDate, 
                    NumberOfDislikes = m.Votes.Count(v => v.VoteType == VoteType.Dislike), 
                    NumberOfLikes = m.Votes.Count(v => v.VoteType == VoteType.Like) 
                });

            return Ok(movies);
        }

        [HttpGet("{id}")]
        [EnableQuery]
        public async Task<IActionResult> Get(int id)
        {
            var movie = await _dbContext.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateMovieRequest movie)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var newMovie = new Movie(movie.Title, movie.Description, movie.PublicationDate, userId);

            _dbContext.Movies.Add(newMovie);

            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = newMovie.Id }, movie);
        }
    }
}
