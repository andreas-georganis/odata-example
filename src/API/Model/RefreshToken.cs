using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Model
{
    public class RefreshToken
    {
        private RefreshToken(string token, int userId, DateTimeOffset createdAt, DateTimeOffset expirationDate)
        {
            Token = token;
            UserId = userId;
            CreatedAt = createdAt;
            ExpirationDate = expirationDate;
        }
        public int Id { get; private set; }
        public string Token { get; private set; }
        public int UserId { get; private set; }
        public DateTimeOffset ExpirationDate { get; private set; } 
        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;

        public bool IsValid()
        {
            return ExpirationDate > DateTimeOffset.UtcNow;
        }

        public static RefreshToken Create(string token, int userId)
        {
            var creationDate = DateTimeOffset.UtcNow;
            var expirationDate = creationDate.AddDays(7);

            return new RefreshToken(token, userId, creationDate, expirationDate);
        }
    }
}
