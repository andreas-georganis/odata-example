using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Model
{
    public class User
    {
        private List<Movie> _submittedMovies;
        private List<Vote> _votes;

        protected User()
        {
            _votes = new List<Vote>();
            _submittedMovies = new List<Movie>();
        }

        public User(string username, string password): this()
        {
            Username = username;
            Password = password;
        }

        public int Id { get; private set; }
        public string Username { get; private set; }

        public string Password { get; private set; }

        public IReadOnlyCollection<Movie> SubmittedMovies => _submittedMovies;
        public IReadOnlyCollection<Vote> Votes => _votes;

        public void Vote(int movieId, VoteType voteType)
        {
            var submittedMovie = _submittedMovies.Find(sm => sm.Id == movieId);

            if (submittedMovie != null)
            {
                throw new Exception("User cannot vote for submitted movie");
            }

            var existingVote = _votes.Find(x => x.MovieId == movieId);

            if (existingVote != null)
            {
                existingVote.SetVoteType(voteType);
                return;
            }
            
            var newVote = new Vote(this.Id, movieId, voteType);

            _votes.Add(newVote);
        }

        public void RetractVote(int movieId)
        {
            var existingVote = _votes.Find(x => x.MovieId == movieId);

            if (existingVote != null)
            {
                throw new Exception("No vote to retract.");
            }

            existingVote.SetRetracted();
        }
    }
}
