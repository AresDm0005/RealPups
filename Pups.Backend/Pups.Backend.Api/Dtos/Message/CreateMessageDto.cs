using System.ComponentModel.DataAnnotations;

namespace Pups.Backend.Api.Dtos.Message;

public record CreateMessageDto
{
    /// <summary>
    /// ID чата которому принадлежит сообщение
    /// </summary>
    /// <example>abcd1234-ab12-ab12-ab12-abcdef123456</example>
    public Guid ChatId { get; init; }

    /// <summary>
    /// ID пользователя, отправившего сообщение
    /// </summary>
    /// <example>abcd1234-ab12-ab12-ab12-abcdef123456</example>
    public Guid SenderId { get; init; }

    /// <summary>
    /// Текст сообщения
    /// </summary>
    /// <example>Привет, мир!</example>
    [Required]
    public string Payload { get; init; } = null!;

    /// <summary>
    /// ID сообщения, на которое отвечает текущее
    /// </summary>
    public int? ReplyTo { get; init; }
}
