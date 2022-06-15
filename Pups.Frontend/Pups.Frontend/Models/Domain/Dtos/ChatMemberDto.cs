namespace Pups.Frontend.Models.Domain.Dtos;

public class ChatMemberDto
{
    public Guid ChatId { get; init; }

    public Guid UserId { get; init; }

    public string? ChatName { get; init; }

    public int ChatStatusId { get; init; }
}