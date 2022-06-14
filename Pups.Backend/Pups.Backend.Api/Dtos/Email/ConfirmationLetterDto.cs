using System.ComponentModel.DataAnnotations;

namespace Pups.Backend.Api.Dtos.Email;

public record ConfirmationLetterDto
{
    /// <summary>
    /// ID пользователя подтверждающего почту
    /// </summary>
    /// <example>abcd1234-ab12-ab12-ab12-abcdef123456</example>
    public Guid UserId { get; set; }

    /// <summary>
    /// Токен подтверждения с кодированием
    /// </summary>
    [Required]
    public string Code { get; set; } = null!;
}
