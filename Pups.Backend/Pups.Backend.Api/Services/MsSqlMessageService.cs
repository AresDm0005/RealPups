using Microsoft.EntityFrameworkCore;
using Pups.Backend.Api.Data;
using Pups.Backend.Api.Models;

namespace Pups.Backend.Api.Services;

public class MsSqlMessageService : IMessageService
{
    private readonly MessengerContext _msgContext;

    public MsSqlMessageService(MessengerContext msgContext)
    {
        _msgContext = msgContext;
    }

    public async Task<Message?> GetMessage(int id)
    {
        return await _msgContext.Messages.FindAsync(id);
    }

    public async Task<IEnumerable<Message>> GetMessages()
    {
        return await _msgContext.Messages
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Message>> GetMessages(Guid chatId)
    {
        return await _msgContext.Messages
            .Where(x => x.ChatId == chatId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task CreateMessage(Message msg)
    {
        _msgContext.Messages.Add(msg);
        await _msgContext.SaveChangesAsync();
    }

    public async Task UpdateMessage(Message msg)
    {
        var existingMessage = await _msgContext.Messages.FindAsync(msg.Id);

        existingMessage!.Payload = msg.Payload;
        existingMessage!.Edited = true;

        _msgContext.Messages.Update(existingMessage);
        await _msgContext.SaveChangesAsync();
    }

    public async Task UpdateChecked(Message msg)
    {
        var existingMessage = await _msgContext.Messages.FindAsync(msg.Id);

        existingMessage!.CheckedAt = msg.CheckedAt;

        _msgContext.Messages.Update(existingMessage);
        await _msgContext.SaveChangesAsync();
    }
}
