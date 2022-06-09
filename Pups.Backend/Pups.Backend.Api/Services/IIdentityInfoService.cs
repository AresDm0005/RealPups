using Pups.Backend.Api.Models;

namespace Pups.Backend.Api.Services;

public interface IIdentityInfoService
{
    Task<bool> DoesUserExist(User user);
    Task DeleteConflictingUser(Guid userId);
}
