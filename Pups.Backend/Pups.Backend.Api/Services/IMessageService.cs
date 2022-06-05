using Pups.Backend.Api.Models;

namespace Pups.Backend.Api.Services;

public interface IMessageService
{
    Task<Message?> GetMessage(int id);
    Task<IEnumerable<Message>> GetMessages();
    Task<IEnumerable<Message>> GetMessages(Guid chatId);
    Task CreateMessage(Message msg);
    Task UpdateMessage(Message msg);
    Task UpdateChecked(Message msg);
}