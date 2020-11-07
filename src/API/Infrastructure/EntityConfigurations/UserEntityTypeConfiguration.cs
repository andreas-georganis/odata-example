using API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Infrastructure.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.Username).IsUnique();

            builder.Property(u => u.Username).HasMaxLength(200).IsRequired();

            builder.Property(u => u.Password).IsRequired();

            var voteNavigation = builder.Metadata.FindNavigation(nameof(User.Votes));

            voteNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var movieNavigation = builder.Metadata.FindNavigation(nameof(User.SubmittedMovies));

            movieNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
