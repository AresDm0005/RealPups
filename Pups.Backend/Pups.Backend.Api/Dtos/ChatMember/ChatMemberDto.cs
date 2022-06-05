namespace Pups.Backend.Api.Dtos.ChatMember;

/// <summary>
/// Представление участника чата
/// </summary>
public record ChatMemberDto
{
    /// <summary>
    /// ID чата
    /// </summary>
    /// <example>abcd1234-ab12-ab12-ab12-abcdef123456</example>
    public Guid ChatId { get; init; }

    /// <summary>
    /// ID пользователя
    /// </summary>
    /// <example>abcd1234-ab12-ab12-ab12-abcdef123456</example>
    public Guid UserId { get; init; }

    /// <summary>
    /// Пользовательское имя (обозначение) чата 
    /// (при null - UserName)
    /// </summary>
    public string? ChatName { get; init; }

    /// <summary>
    /// ID пользовательского статуса чата (Обычный/избранный/ЧС/т.д.)
    /// </summary>
    public int ChatStatusId { get; init; }
}
