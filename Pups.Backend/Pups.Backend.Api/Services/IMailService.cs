using Pups.Backend.Api.Models;

namespace Pups.Backend.Api.Services;

public interface IMailService
{
    Task<bool> SendEmailConfirmationLetter(User user, string code);
    Task<bool> SendEmailChangeConfirmationLetter(User user, string newEmail, string code);
}
