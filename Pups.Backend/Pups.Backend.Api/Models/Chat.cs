namespace Pups.Backend.Api.Models;

public partial class Chat
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int TypeId { get; set; }

    public virtual User Creator { get; set; } = null!;
    public virtual ChatType Type { get; set; } = null!;
    public virtual ICollection<ChatMember>? Members { get; set; }
    public virtual ICollection<Message>? Messages { get; set; }
}
