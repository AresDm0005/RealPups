using System.ComponentModel.DataAnnotations;

namespace Pups.Backend.Api.Dtos.Message;

public record UpdateMessageDto
{
    /// <summary>
    /// Отредактированный текст сообщения
    /// </summary>
    /// <example>Привет, мир!</example>
    [Required]
    public string Payload { get; init; } = null!;
}
