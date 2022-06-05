using Microsoft.EntityFrameworkCore;
using Pups.Backend.Api.Data;
using Pups.Backend.Api.Models;

namespace Pups.Backend.Api.Services;

public class MsSqlUserService : IUserService
{
    private readonly MessengerContext _msgContext;

    public MsSqlUserService(MessengerContext msgContext)
    {
        _msgContext = msgContext;
    }

    public async Task<bool> DoesUserExist(Guid userId)
    {
        return await _msgContext.Users.AnyAsync(x => x.Id == userId);
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _msgContext.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<User?> GetUser(Guid id)
    {
        return await _msgContext.Users.FindAsync(id);
    }

    public async Task CreateUser(User user)
    {
        _msgContext.Users.Add(user);
        await _msgContext.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
        var existingUser = await GetUser(user.Id) 
            ?? throw new Exception($"User with id = {user.Id} does not exist and can't be updated");

        existingUser.Username = user.Username;
        existingUser.Email = user.Email;
        existingUser.Info = user.Info;
        existingUser.LastSeen = user.LastSeen;

        _msgContext.Users.Update(existingUser);
        await _msgContext.SaveChangesAsync();
    }
}
