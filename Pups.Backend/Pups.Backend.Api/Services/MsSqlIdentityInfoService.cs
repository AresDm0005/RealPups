using Pups.Backend.Api.Data;
using Pups.Backend.Api.Models;

namespace Pups.Backend.Api.Services;

public class MsSqlIdentityInfoService : IIdentityInfoService
{
    private readonly IdentityContext _identityContext;

    public MsSqlIdentityInfoService(IdentityContext identityContext)
    {
        _identityContext = identityContext;
    }

    public async Task<bool> DoesUserExist(User user)
    {
        var existingUser = await _identityContext.Users.FindAsync(user.Id.ToString());

        if (existingUser == null)
            return false;

        return existingUser.UserName.Equals(user.Username)
            && existingUser.Email.Equals(user.Email);
    }

    public async Task DeleteConflictingUser(Guid userId)
    {
        var user = await _identityContext.Users.FindAsync(userId.ToString());

        if (user is null)
            return;

        _identityContext.Users.Remove(user);
        await _identityContext.SaveChangesAsync();
    }
}
