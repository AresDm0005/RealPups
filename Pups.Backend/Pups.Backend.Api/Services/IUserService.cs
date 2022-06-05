using Pups.Backend.Api.Models;

namespace Pups.Backend.Api.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsers();
    Task<User?> GetUser(Guid id);
    Task CreateUser(User user);
    Task UpdateUser(User user);
    Task<bool> DoesUserExist(Guid userId);
}
