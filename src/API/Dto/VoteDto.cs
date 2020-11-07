using API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto
{
    public class VoteDto
    {
        public int MovieId { get; set; } 

        public VoteType Type { get; set; }
    }
}
