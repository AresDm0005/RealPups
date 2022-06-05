using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pups.Backend.Api.Models.Configuration;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Message");

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id");

        builder.Property(e => e.ChatId).HasColumnName("chat_id");

        builder.Property(e => e.CheckedAt)
            .HasColumnType("datetime")
            .HasColumnName("checked_at");

        builder.Property(e => e.Edited).HasColumnName("edited");

        builder.Property(e => e.Payload)
            .HasColumnType("text")
            .HasColumnName("payload");

        builder.Property(e => e.ReplyTo).HasColumnName("reply_to");

        builder.Property(e => e.SendAt)
            .HasColumnType("datetime")
            .HasColumnName("send_at");

        builder.Property(e => e.SenderId).HasColumnName("sender_id");

        builder.HasOne(d => d.Chat)
            .WithMany(p => p.Messages)
            .HasForeignKey(d => d.ChatId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Message_Chat");

        builder.HasOne(d => d.ReplyToNavigation)
            .WithMany(p => p.RepliesToThisMessage)
            .HasForeignKey(d => d.ReplyTo)
            .HasConstraintName("FK_Message_Message");

        builder.HasOne(d => d.Sender)
            .WithMany(p => p.Messages)
            .HasForeignKey(d => d.SenderId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Message_User");
    }
}
