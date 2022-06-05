using Microsoft.EntityFrameworkCore;
using Pups.Backend.Api.Data;
using Pups.Backend.Api.Models;

namespace Pups.Backend.Api.Services;

public class MsSqlChatMemberService : IChatMemberService
{
    private readonly MessengerContext _msgContext;

    public MsSqlChatMemberService(MessengerContext msgContext)
    {
        _msgContext = msgContext;
    }

    public async Task<IEnumerable<ChatMember>> GetAllChatMembers()
    {
        return await _msgContext.ChatMembers
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ChatMember?> GetChatMember(Guid chatId, Guid userId)
    {
        return await _msgContext.ChatMembers
            .FindAsync(chatId, userId);
    }

    public async Task<IEnumerable<ChatMember>> GetChatMembers(Guid chatId)
    {
        return await _msgContext.ChatMembers
            .Where(x => x.ChatId == chatId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task CreateChatMember(ChatMember member)
    {
        _msgContext.ChatMembers.Add(member);
        await _msgContext.SaveChangesAsync();
    }

    public async Task UpdateChatMember(ChatMember member)
    {
        _msgContext.ChatMembers.Update(member);
        await _msgContext.SaveChangesAsync();
    }
}
