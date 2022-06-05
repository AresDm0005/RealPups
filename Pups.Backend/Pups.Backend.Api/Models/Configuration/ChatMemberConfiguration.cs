using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pups.Backend.Api.Models.Configuration;

public class ChatMemberConfiguration : IEntityTypeConfiguration<ChatMember>
{
    public void Configure(EntityTypeBuilder<ChatMember> builder)
    {
        builder.HasKey(x => new { x.ChatId, x.UserId });

        builder.ToTable("ChatMember");

        builder.Property(e => e.ChatId).HasColumnName("chat_id");

        builder.Property(e => e.ChatName)
            .HasMaxLength(50)
            .HasColumnName("chat_name");

        builder.Property(e => e.ChatStatusId).HasColumnName("chat_status");

        builder.Property(e => e.UserId).HasColumnName("user_id");

        builder.HasOne(d => d.Chat)
            .WithMany(c => c.Members)
            .HasForeignKey(d => d.ChatId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ChatMember_Chat");

        builder.HasOne(d => d.ChatStatusNavigation)
            .WithMany()
            .HasForeignKey(d => d.ChatStatusId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ChatMember_ChatStatus");

        builder.HasOne(d => d.User)
            .WithMany(u => u.Memberships)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ChatMember_User");
    }
}
