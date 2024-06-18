using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AttendanceManagement.Services;

public class EmailSender : IEmailSender
{
    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
    {
        Options = optionsAccessor.Value;
    }

    public AuthMessageSenderOptions Options { get; }

    public Task SendEmailAsync(string email, string subject, string message)
    {
        Options.SendGridKey = "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";
        return Execute(Options.SendGridKey, subject, message, email);
    }

    private Task Execute(string optionsSendGridKey, string subject, string message, string email)
    {
        var client = new SendGridClient(optionsSendGridKey);
        var msg = new SendGridMessage
        {
            From = new EmailAddress("sahil242bhatiya@gmail.com", "Sahil Bhatiya"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(email));
        msg.SetClickTracking(false, false);

        return client.SendEmailAsync(msg);
    }
}