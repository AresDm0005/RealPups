using System.ComponentModel.DataAnnotations;

namespace Pups.Backend.Api.Dtos.User;

/// <summary>
/// Данные пользователя
/// </summary>
public record UserDto
{
    /// <summary>
    /// ID пользователя
    /// </summary>
    /// <example>abcd1234-ab12-ab12-ab12-abcdef123456</example>
    public Guid Id { get; init; }

    /// <summary>
    /// Имя пользователя
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
    /// Био/информация о пользователе
    /// </summary>
    /// <example>Колян из Перми</example>
    public string? Info { get; init; }

    /// <summary>
    /// Дата регистрации
    /// </summary>
    public DateTime Created { get; init; }

    /// <summary>
    /// Дата последнего онлайна
    /// </summary>
    public DateTime LastSeen { get; init; }
}
