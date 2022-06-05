using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pups.Backend.Api.Models.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("(newid())");

        builder.Property(e => e.Created)
            .HasColumnType("datetime")
            .HasColumnName("created");

        builder.Property(e => e.Email).HasColumnName("email");

        builder.Property(e => e.Info).HasColumnName("info");

        builder.Property(e => e.LastSeen)
            .HasColumnType("datetime")
            .HasColumnName("last_seen");

        builder.Property(e => e.Username)
            .HasMaxLength(50)
            .HasColumnName("username");
    }
}
