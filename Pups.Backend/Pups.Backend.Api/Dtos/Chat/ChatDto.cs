using Pups.Backend.Api.Dtos.ChatMember;

namespace Pups.Backend.Api.Dtos.Chat;

/// <summary>
/// Информация о чате
/// </summary>
public record ChatDto
{
    /// <summary>
    /// ID чата
    /// </summary>
    /// <example>abcd1234-ab12-ab12-ab12-abcdef123456</example>
    public Guid Id { get; init; }

    /// <summary>
    /// Дата создания чата
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// ID типа чата (1 - один-на-один, 2 - групповой, 3 - одиночный канал)
    /// </summary>
    public int TypeId { get; init; }

    /// <summary>
    /// Участники чата
    /// </summary>
    public ICollection<ChatMemberDto>? Members { get; init; }
}
