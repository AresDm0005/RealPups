namespace Pups.Backend.Api.Dtos.Chat;

public record CreateChatDto
{
    /// <summary>
    /// ID пользователя, инициирующего создание чата
    /// </summary>
    /// <example>abcd1234-ab12-ab12-ab12-abcdef123456</example>
    public Guid CreatorId { get; init; }

    /// <summary>
    /// Название чата (персонализированное имя контакта / спец. имя) присвоенное создателем чату
    /// </summary>
    /// <remarks>
    /// Относится только к создателю - для собеседника чат будет обозначаться юзернеймом создателя
    /// </remarks>
    public string? CreatorChatName { get; init; }

    /// <summary>
    /// Статус чата для создателя (обычный, избранный, ...)
    /// </summary>
    /// <remarks>
    /// Относится только к создателю - для собеседника чат будет обозначаться как обычный
    /// </remarks>
    public int? CreatorChatStatusId { get; init; }

    /// <summary>
    /// ID пользователя с которым инициируется чат
    /// </summary>
    /// <example>abcd1234-ab12-ab12-ab12-abcdef123456</example>
    public Guid ContactId { get; init; }
}
