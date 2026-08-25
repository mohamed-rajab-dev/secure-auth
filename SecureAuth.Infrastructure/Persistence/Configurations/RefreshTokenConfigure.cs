using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureAuth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecureAuth.Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfigure : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Token);
            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(rt => rt.ExpiresAt)
                .IsRequired();
            builder.Property(rt => rt.CreatedAt)
            .   IsRequired();
        }
    }
}
