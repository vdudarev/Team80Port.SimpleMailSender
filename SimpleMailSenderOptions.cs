using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team80Port.SimpleMailSender
{
    /// <summary>
    /// настройки почтовой компоненты
    /// </summary>
    public class SimpleMailSenderOptions
    {
        public string SmtpHost { get; set; } = "localhost";
        public int SmtpPort { get; set; } = 25;

        public string FromName { get; set; } = string.Empty;
        public string FromEmail { get; set; } = "noreply@mail.ru";


        public string? CredentialsUserName { get; set; }
        public string? CredentialsPassword { get; set; }
        public string? CredentialsDomain { get; set; }
    }
}
