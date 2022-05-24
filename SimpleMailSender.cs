using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;

namespace Team80Port.SimpleMailSender
{
    public class SimpleMailSender : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
    {
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public SimpleMailSenderOptions Options { get; } // Set with Secret Manager.

        public SimpleMailSender(Microsoft.Extensions.Options.IOptions<SimpleMailSenderOptions> optionsAccessor,
            ILogger<SimpleMailSender> logger)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using MailMessage message = new MailMessage();
            message.To.Add(new MailAddress(email));
            if (string.IsNullOrEmpty(Options.FromName))
            {
                message.From = new MailAddress(Options.FromEmail);
            }
            else
            {
                message.From = new MailAddress(Options.FromEmail, Options.FromName);
            }
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = htmlMessage;

            using SmtpClient client = new SmtpClient(Options.SmtpHost);
            if (Options.SmtpPort > 0)
            {
                client.Port = Options.SmtpPort;
            }
            if (string.IsNullOrEmpty(Options.CredentialsUserName))
            {
                client.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
            else
            {
                client.Credentials = new NetworkCredential(Options.CredentialsUserName, Options.CredentialsPassword, Options.CredentialsDomain);
            }
            await client.SendMailAsync(message);
            _logger?.LogInformation($"Email to {email} from {Options.FromEmail} sent successfully!");
        }

        /// <summary>
        /// Отправка сообщения по E-Mail через стандартную компоненту дотнета
        /// TODO: удалить - это просто тест для проверки базовой работоспособности
        /// </summary>
        /// <param name="fromEmail">мыло от кого</param>
        /// <param name="fromName">имя от кого</param>
        /// <param name="toEmail">мыло куда</param>
        /// <param name="toName">имя куда</param>
        /// <param name="subject">тема</param>
        /// <param name="body">сообщение</param>
        /// <param name="IsBodyHtml">формат (true - HTML, false - просто текст)</param>
        public static void Send(string fromEmail, string fromName, string toEmail, string toName,
            string subject, string body, bool IsBodyHtml)
        {
            using MailMessage message = new MailMessage();
            message.To.Add(new MailAddress(toEmail, toName));
            message.From = new MailAddress(fromEmail, fromName);
            message.Subject = subject;
            message.IsBodyHtml = IsBodyHtml;
            message.Body = body;

            using SmtpClient client = new SmtpClient("localhost");
            client.Credentials = CredentialCache.DefaultNetworkCredentials;
            client.Send(message);
        }

    }
}