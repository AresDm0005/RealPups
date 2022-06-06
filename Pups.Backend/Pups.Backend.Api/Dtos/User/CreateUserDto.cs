using System.ComponentModel.DataAnnotations;

namespace Pups.Backend.Api.Dtos.User;

public record CreateUserDto
{
    /// <summary>
    /// ID пользователя
    /// </summary>
    /// <example>abcd1234-ab12-ab12-ab12-abcdef123456</example>
    public Guid Id { get; init; }
    /// <summary>
    /// Имя создаваемого пользователя
    /// </summary>
    /// <example>Kolya12345</example>
    [Required]
    public string Username { get; init; } = null!;

    /// <summary>
    /// Электронная почта
    /// </summary>
    /// <example>example@example.com</example>
    [Required]
    public string Email { get; init; } = null!;

    /// <summary>
    /// Био/информация о создаваемом пользователе
    /// </summary>
    /// <example>Колян из Перми</example>
    public string? Info { get; init; }
}
