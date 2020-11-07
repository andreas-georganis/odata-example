using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Model
{
    public enum VoteType
    {
        Like,
        Dislike
    }

    public enum VoteState
    {
        Active,
        Retracted
    }

    public class Vote
    {
        public Vote(int userId, int movieId, VoteType voteType)
        {
            UserId = userId;
            MovieId = movieId;
            VoteType = voteType;
            VoteState = VoteState.Active;
        }

        public int Id { get; private set; }

        public int UserId { get; private set; }

        public int MovieId { get; private set; }

        public VoteType VoteType { get; private set; }

        public VoteState VoteState { get; private set; }

        public void SetRetracted()
        {
            if (this.VoteState == VoteState.Active)
            {
                this.VoteState = VoteState.Retracted;
            }
        }

        public void SetVoteType(VoteType voteType)
        {
            VoteType = voteType;
        }
    }
}
