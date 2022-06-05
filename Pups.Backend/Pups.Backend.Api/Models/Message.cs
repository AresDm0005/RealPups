namespace Pups.Backend.Api.Models;

public partial class Message
{
    public int Id { get; set; }
    public Guid ChatId { get; set; }
    public Guid SenderId { get; set; }
    public string Payload { get; set; } = null!;
    public DateTime SendAt { get; set; }
    public DateTime? CheckedAt { get; set; }
    public bool Edited { get; set; }
    public int? ReplyTo { get; set; }

    public virtual Chat Chat { get; set; } = null!;
    public virtual Message? ReplyToNavigation { get; set; }
    public virtual User Sender { get; set; } = null!;
    public virtual ICollection<Message>? RepliesToThisMessage { get; set; }
}
