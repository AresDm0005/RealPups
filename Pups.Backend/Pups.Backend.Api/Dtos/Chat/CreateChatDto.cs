using System.ComponentModel.DataAnnotations;

namespace Pups.Backend.Api.Dtos.Chat;

public record CreateChatDto
{
    /// <summary>
    /// ID участников нового чата
    /// </summary>
    [Required]
    public ICollection<Guid> MembersIds { get; init; } = null!;
}