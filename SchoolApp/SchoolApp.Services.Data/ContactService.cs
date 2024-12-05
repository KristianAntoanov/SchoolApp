using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using SchoolApp.Services.Data.Contrancts;
using SchoolApp.Web.ViewModels.Home;

namespace SchoolApp.Services.Data
{
    public class ContactService : IContactService
    {
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public ContactService(IConfiguration configuration, IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _configuration = configuration;
        }

        public async Task<bool> SubmitContactFormAsync(ContactFormModel model)
        {
            if (model == null)
            {
                return false;
            }

            string subject = $"Съобщение от {model.Name}: {model.Subject}";

            string htmlMessage = $@"
            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;'>
                <h2 style='color: #dc3545;'>Ново съобщение от контактната форма</h2>
                <div style='margin: 20px 0;'>
                    <p style='margin-bottom: 5px;'><strong>Име:</strong> {WebUtility.HtmlEncode(model.Name)}</p>
                    <p style='margin-bottom: 5px;'><strong>Имейл:</strong> {WebUtility.HtmlEncode(model.Email)}</p>
                    <p style='margin-bottom: 5px;'><strong>Относно:</strong> {WebUtility.HtmlEncode(model.Subject)}</p>
                </div>
                <div style='margin: 20px 0;'>
                    <p style='margin-bottom: 5px;'><strong>Съобщение:</strong></p>
                    <p style='background-color: #f8f9fa; padding: 15px; border-radius: 5px;'>{WebUtility.HtmlEncode(model.Message)}</p>
                </div>
                <hr style='margin: 20px 0;'>
                <p style='color: #666; font-size: 12px;'>
                    Това съобщение е изпратено от контактната форма на уебсайта на СГСАГ
                </p>
                <p style = 'color: #666; font-size: 12px;'>
                    Дата на изпращане: { DateTime.Now:dd.MM.yyyy HH:mm}
                </p>
            </div> ";

            string? schoolEmail = _configuration["SendGrid:FromEmail"];

            if (schoolEmail == null)
            {
                return false;
            }

            await _emailSender.SendEmailAsync(schoolEmail, subject, htmlMessage);

            return true;
        }
    }
}