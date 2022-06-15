namespace Pups.Frontend.Models.Domain.Dtos;

public record chatDto
{
    public Guid Id { get; init; }

    public DateTime CreatedAt { get; init; }

    public int TypeId { get; init; }

    public ICollection<ChatMemberDto>? Members { get; init; }
}