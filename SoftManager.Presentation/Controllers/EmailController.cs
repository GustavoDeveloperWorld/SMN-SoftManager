using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SoftManager.Presentation.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailSender _emailSender;

        public EmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task<IActionResult> SendTestEmail()
        {
            await _emailSender.SendEmailAsync("recipient@example.com", "Test Email", "<h1>Hello from SoftManager!</h1>");
            return Content("Email enviado com sucesso!");
        }
    }
}
