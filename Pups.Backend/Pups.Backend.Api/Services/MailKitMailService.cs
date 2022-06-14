using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Pups.Backend.Api.Emails.Models;
using Pups.Backend.Api.Models;

namespace Pups.Backend.Api.Services;

public class MailKitMailService : IMailService
{
    private readonly MailSettings _mailSettings;

    public MailKitMailService(IOptions<MailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }

    public async Task<bool> SendEmailConfirmationLetter(User user, string code)
    {
        try
        {
            var temlateBody = File.ReadAllText(MailStandardStrings.EmailConfirmationTemplatePath);

            if (temlateBody is null)
                return false;

            temlateBody = temlateBody
                .Replace("%confirm_url%", $"https://localhost:7128/Identity/Account/ConfirmEmail?userId={user.Id}&code={code}&returnUrl=%2F")
                .Replace("%Username%", user.Username);            

            var builder = new BodyBuilder
            {
                HtmlBody = temlateBody
            };

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Email);
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = MailStandardStrings.EmailConfirmationSubject;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.Email, _mailSettings.Password);
            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        } 
        catch { return false; }

        return true;
    }

    
    public async Task<bool> SendEmailChangeConfirmationLetter(User user, string newEmail, string code)
    {
        try
        {
            var temlateBody = File.ReadAllText(MailStandardStrings.EmailChangeConfirmationTemplatePath);

            if (temlateBody is null)
                return false;

            temlateBody = temlateBody
                .Replace("%confirm_url%", $"https://localhost:7128/Identity/Account/ConfirmEmailChange?userId={user.Id}&email={newEmail}&code={code}&returnUrl=%2F")
                .Replace("%Username%", user.Username);

            var builder = new BodyBuilder
            {
                HtmlBody = temlateBody
            };

            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = MailStandardStrings.EmailChangeConfirmationSubject,
                Body = builder.ToMessageBody()
            };
            email.To.Add(MailboxAddress.Parse(newEmail));            

            using var smtp = new SmtpClient();

            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.Email, _mailSettings.Password);
            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }
        catch { return false; }

        return true;
    }    
}
