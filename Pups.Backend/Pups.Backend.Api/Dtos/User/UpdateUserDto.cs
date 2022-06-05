namespace Pups.Backend.Api.Dtos.User;

public record UpdateUserDto
{
    /// <summary>
    /// Новое имя пользователя
    /// </summary>
    /// <example>Kolya12345</example>
    public string? Username { get; init; }

    /// <summary>
    /// Обновленное био/информация о пользователе
    /// </summary>
    /// <example>Колян из Перми</example>
    public string? Info { get; init; }

    /// <summary>
    /// Дата последнего онлайна
    /// </summary>
    public DateTime? LastSeen { get; init; }
}
