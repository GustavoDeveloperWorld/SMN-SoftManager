using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SoftManager.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string _email, string subject, string body)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);
            var password = _configuration["EmailSettings:Password"];
            String SendMailFrom = _configuration["EmailSettings:SenderEmail"];
            String SendMailTo = _email;

            //using var smtpClient = new SmtpClient(smtpServer)
            //{
            //    Port = port,
            //    Credentials = new NetworkCredential(senderEmail, password),
            //    EnableSsl = true,
            //};

            //var mailMessage = new MailMessage
            //{
            //    From = new MailAddress(senderEmail),
            //    Subject = subject,
            //    Body = body,
            //    IsBodyHtml = true,
            //};

            //mailMessage.To.Add(email);

            //await smtpClient.SendMailAsync(mailMessage);


            //String SendMailSubject = "Email Subject";
           // String SendMailBody = "Email Body";

            try
            {
                SmtpClient SmtpServer = new SmtpClient(smtpServer, port);
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage email = new MailMessage();
                // START
                email.From = new MailAddress(SendMailFrom);
                email.To.Add(SendMailTo);
                //email.CC.Add(SendMailFrom);
                email.Subject = subject;
                email.Body = body;
                ////END
                SmtpServer.Timeout = 5000;
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(SendMailFrom, password);
                SmtpServer.Send(email);


                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }
    }

}
