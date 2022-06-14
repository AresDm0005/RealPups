namespace Pups.Backend.Api.Emails.Models;

public static class MailStandardStrings
{
    public static string ContentRootPath = "";

    public static string EmailConfirmationTemplatePath =>
        Path.Combine(Path.GetDirectoryName(ContentRootPath)!, @"Emails\Templates\EmailConfirmation.html");

    public static readonly string EmailConfirmationSubject =
        "Подтверждение почты в мессенджере PUPS";

    public static string EmailChangeConfirmationTemplatePath =>
        Path.Combine(Path.GetDirectoryName(ContentRootPath)!, @"Emails\Templates\EmailChangeConfirmation.html");

    public static readonly string EmailChangeConfirmationSubject =
        "Подтверждение новой почты в мессенджере PUPS";

}
