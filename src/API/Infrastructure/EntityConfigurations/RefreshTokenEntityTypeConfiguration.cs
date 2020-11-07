using API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Infrastructure.EntityConfigurations
{
    public class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne<User>().WithMany().HasForeignKey(x => x.UserId);

            builder.Property(x => x.Token).IsRequired();

            builder.Property(x => x.CreatedAt).IsRequired();

            builder.Property(x => x.ExpirationDate).IsRequired();

        }
    }
}
