using System.ComponentModel.DataAnnotations;

namespace Pups.Backend.Api.Dtos.Message;

public record MessageDto
{
    /// <summary>
    /// ID сообщения
    /// </summary>
    public int Id { get; init; }

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
    /// Дата отправки сообщения
    /// </summary>
    public DateTime SendAt { get; init; }

    /// <summary>
    /// Дата прочтения сообщения
    /// </summary>
    public DateTime? CheckedAt { get; init; }

    /// <summary>
    /// Редактировалось ли сообщение
    /// </summary>
    public bool Edited { get; init; }

    /// <summary>
    /// ID сообщения, на которое отвечает текущее
    /// </summary>
    public int? ReplyTo { get; init; }
}
