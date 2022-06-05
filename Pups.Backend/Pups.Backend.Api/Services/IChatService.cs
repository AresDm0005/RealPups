using Pups.Backend.Api.Models;

namespace Pups.Backend.Api.Services;

public interface IChatService
{
    Task<Chat?> GetChat(Guid id, bool includeMembers = false);
    Task<IEnumerable<Chat>> GetChats();
    Task<IEnumerable<Chat>> GetChats(Guid userId);
    Task CreateChatAndMembers(Chat chat, IEnumerable<ChatMember> chatMembers);
    Task<bool> DoesChatExistWithMembers(ICollection<Guid> membersIds);
    Task<bool> IsUserAChatMember(Guid senderId, Guid chatId);
}
