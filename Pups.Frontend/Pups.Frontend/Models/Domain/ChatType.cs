namespace Pups.Frontend.Models.Domain;

public partial class ChatType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public virtual ICollection<Chat>? Chats { get; set; }
}
