using System.ComponentModel.DataAnnotations;

namespace Pups.Backend.Api.Dtos.Chat;

public record CreateGroupDto
{
    /// <summary>
    /// ID пользователя, инициирующего создание беседы
    /// </summary>
    /// <example>abcd1234-ab12-ab12-ab12-abcdef123456</example>
    public Guid CreatorId { get; init; }

    /// <summary>
    /// Статус беседы для создателя (обычный, избранный, ...)
    /// </summary>
    /// <remarks>
    /// Относится только к создателю - для участников беседы она будет обозначаться как обычная
    /// </remarks>
    public int? CreatorChatStatusId { get; init; }

    /// <summary>
    /// Название (обозначение) беседы
    /// </summary>
    [Required]
    public string GroupName { get; init; } = null!;

    /// <summary>
    /// ID участников новой беседы
    /// </summary>
    [Required]
    public ICollection<Guid> MembersIds { get; init; } = null!;
}