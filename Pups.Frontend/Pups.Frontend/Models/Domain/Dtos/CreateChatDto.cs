namespace Pups.Frontend.Models.Domain.Dtos;

public class CreateChatDto
{
    public Guid CreatorId { get; init; }
    public string? CreatorChatName { get; init; }
    public int? CreatorChatStatusId { get; init; }
    public Guid ContactId { get; init; }
}