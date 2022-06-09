using System.ComponentModel.DataAnnotations;

namespace Pups.Backend.Api.Dtos.Chat;

public record CreateSoloChatDto
{
    /// <summary>
    /// ID пользователя, для которого создается одиночный чат
    /// </summary>
    /// <example>abcd1234-ab12-ab12-ab12-abcdef123456</example>
    [Required]
    public Guid CreatorId { get; init; }
}