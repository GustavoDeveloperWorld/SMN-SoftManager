using Microsoft.Extensions.Options;
using SoftManager.Infrastructure.Settings;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                var smtpClient = new SmtpClient(_emailSettings.SmtpServer)
                {
                    Port = _emailSettings.Port,
                    Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password),
                    EnableSsl = true,
                };

                //var mailMessage = new MailMessage
                //{
                //    From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                //    Subject = subject,
                //    Body = htmlMessage,
                //    IsBodyHtml = true,
                //};

                //mailMessage.To.Add(email);

                //await smtpClient.SendMailAsync(mailMessage);
            }
            else
            {
                throw new ArgumentNullException(nameof(email), "O e-mail não pode estar vazio.");
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Erro ao enviar e-mail.", ex);
        }
    }
}
