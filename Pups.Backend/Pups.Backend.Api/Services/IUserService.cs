using Pups.Backend.Api.Models;

namespace Pups.Backend.Api.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsers();
    Task<User?> GetUser(Guid id);
    Task<IEnumerable<User>> FindUser(string userName);
    Task CreateUser(User user);
    Task UpdateUser(User user);
    Task<bool> DoesUserExist(Guid userId);
    Task<bool> IsUserNameNew(string userName);
}
