namespace Pups.Backend.Api.Models;

public partial class ChatMember
{
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
    public string? ChatName { get; set; }
    public int ChatStatusId { get; set; }

    public virtual Chat Chat { get; set; } = null!;
    public virtual ChatStatus ChatStatusNavigation { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
