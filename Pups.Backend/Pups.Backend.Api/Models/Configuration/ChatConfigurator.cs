using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pups.Backend.Api.Models.Configuration
{
    public class ChatConfigurator : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.ToTable("Chat");

            builder.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");

            builder.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");

            builder.Property(e => e.TypeId)
                .HasColumnName("type_id");

            builder.HasOne(d => d.Type)
                .WithMany(p => p.Chats)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chat_ChatType");            
        }
    }
}
