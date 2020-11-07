using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTimeOffset PublicationDate { get; set; }

        public int NumberOfLikes { get; set; }

        public int NumberOfDislikes { get; set; }
    }
}
