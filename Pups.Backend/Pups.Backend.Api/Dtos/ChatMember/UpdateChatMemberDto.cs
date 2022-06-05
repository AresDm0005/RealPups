namespace Pups.Backend.Api.Dtos.ChatMember;

public record UpdateChatMemberDto
{
    /// <summary>
    /// Обновленное пользовательское имя (обозначение) чата 
    /// (при null - UserName)
    /// </summary>
    public string? ChatName { get; set; }

    /// <summary>
    /// ID пользовательского статуса чата (Обычный/избранный/ЧС/т.д.)
    /// </summary>
    public int? ChatStatusId { get; set; }
}
