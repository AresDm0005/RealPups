using Microsoft.EntityFrameworkCore;
using Pups.Backend.Api.Data;
using Pups.Backend.Api.Models;

namespace Pups.Backend.Api.Services;

public class MsSqlChatService : IChatService
{
    private readonly MessengerContext _msgContext;

    public MsSqlChatService(MessengerContext msgContext)
    {
        _msgContext = msgContext;
    }

    public async Task<Chat?> GetChat(Guid id, bool includeMembers = false)
    {
        if (includeMembers)
            return await _msgContext.Chats
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Id == id);
    
        return await _msgContext.Chats.FindAsync(id);
    }

    public async Task<IEnumerable<Chat>> GetChats()
    {
        var chats = await _msgContext.Chats
            .AsNoTracking()
            .ToListAsync();

        return chats;
    }

    public async Task<IEnumerable<Chat>> GetChats(Guid userId)
    {
        return await _msgContext.ChatMembers
            .Include(x => x.Chat)
            .Where(x => x.UserId == userId)
            .Select(x => x.Chat)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task CreateChatAndMembers(Chat chat, IEnumerable<ChatMember> chatMembers)
    {
        _msgContext.Add(chat);
        _msgContext.AddRange(chatMembers);
        await _msgContext.SaveChangesAsync();
    }

    public Task<bool> DoesChatExistWithMembers(ICollection<Guid> membersIds)
    {
        if (membersIds.Count() < 1 || membersIds.Count() > 2)
            throw new ArgumentException("Test for existing chat with particular users can be done only  for 1 or 2 users");

        return membersIds.Count() == 1 ? CheckOneUser(membersIds.First()) : CheckTwoUsers(membersIds);
    }

    public async Task<bool> IsUserAChatMember(Guid userId, Guid chatId)
    {
        var chat = await GetChat(chatId, includeMembers: true);
        if (chat == null)
            return false;

        return chat.Members!.Any(x => x.UserId == userId);
    }

    private Task<bool> CheckOneUser(Guid userId)
    {
        return _msgContext.ChatMembers
            .GroupBy(x => x.ChatId)
            .Where(x => x.Count() == 1)
            .AnyAsync(x => x.First().UserId == userId);
    }

    private Task<bool> CheckTwoUsers(ICollection<Guid> userIds)
    {
        return _msgContext.ChatMembers
            .GroupBy(x => x.ChatId)
            .Where(x => x.Count() == 2)
            .Select(x => x
                .Select(y => y.UserId)
                .ToHashSet())
            .AnyAsync(x => x.SetEquals(userIds));
    }
}
