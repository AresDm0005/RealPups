namespace Pups.Backend.Api.Models;

public partial class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Info { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastSeen { get; set; }

    public virtual ICollection<ChatMember>? Memberships { get; set; }
    public virtual ICollection<Message>? Messages { get; set; }
}
