namespace Pups.Backend.Api.Emails.Models;

public record MailSettings
{
    public string Email { get; init; } = null!;
    public string DisplayName { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string Host { get; init; } = null!;
    public int Port { get; set; }
}
