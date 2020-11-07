using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Model
{
    public class Movie
    {
        public Movie(string title, string description, DateTimeOffset publicationDate, int userId)
        {
            Title = title;
            Description = description;
            PublicationDate = publicationDate;
            UserId = userId;
        }

        public int Id { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public int UserId { get; private set; }

        public DateTimeOffset PublicationDate { get; private set; }

        public List<Vote> Votes { get; private set; } = new List<Vote>();
    }
}
