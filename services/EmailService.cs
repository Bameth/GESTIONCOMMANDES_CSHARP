using System.Net;
using System.Net.Mail;
namespace GESTIONCOMMANDES.services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpHost = _configuration["EmailSettings:SmtpHost"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var senderName = _configuration["EmailSettings:SenderName"];
                var senderPassword = _configuration["EmailSettings:SenderPassword"];

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(toEmail);

                using var smtpClient = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true
                };

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Ajoutez la journalisation ici
                Console.WriteLine($"Erreur lors de l'envoi de l'e-mail : {ex.Message}");
                throw; // Relancer l'exception pour la capturer à un niveau supérieur
            }
        }
    }
}