namespace Pups.Frontend.Models.Domain.Dtos;

public class ChatPreview
{
    public Guid Id { get; set; }
    public string ChatName { get; set; }
    public Message Message { get; set; }
    
    public ICollection<ChatMember>? Members { get; set; }
}