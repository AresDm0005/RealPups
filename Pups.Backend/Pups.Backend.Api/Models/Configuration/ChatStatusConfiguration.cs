using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pups.Backend.Api.Models.Configuration
{
    public class ChatStatusConfiguration : IEntityTypeConfiguration<ChatStatus>
    {
        public void Configure(EntityTypeBuilder<ChatStatus> builder)
        {
            builder.ToTable("ChatStatus");

            builder.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");

            builder.Property(e => e.Title).HasColumnName("title");
        }
    }
}
