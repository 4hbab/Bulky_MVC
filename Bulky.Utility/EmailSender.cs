using Microsoft.AspNetCore.Identity.UI.Services;

namespace Bulky.Utility;

// EmailSender nodig om error te vermijden
// > wanneer in program.cs AddDefaultIdentity wordt vervangen door AddIdentity
// > Bij AddDefaultIdentity wél een (fake) default implementatie gedaan van de emailSender
// ... waardoor er geen error wordt gegeven bij het instantiëren van de IEmailSender in de RegistrationPage
// > Om deze foutmeling te vermijden is er een implementatie nodig van IEmailSender
// V.8:45:06 & 8:48:30
public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage) {
        // In future > Logic to send email
        return Task.CompletedTask;
    }
}
