using Pups.Backend.Api.Models;

namespace Pups.Backend.Api.Services;

public interface IChatMemberService
{
    Task<ChatMember?> GetChatMember(Guid chatId, Guid userId);
    Task<IEnumerable<ChatMember>> GetAllChatMembers();
    Task<IEnumerable<ChatMember>> GetChatMembers(Guid chatId);
    Task CreateChatMember(ChatMember member);
    Task UpdateChatMember(ChatMember member);
}
