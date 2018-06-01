using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Utils.Net.Interfaces;

namespace Utils.Net.Managers
{
    /// <summary>
    /// Provides mail sending functionality.
    /// </summary>
    public class MailManager : IMailManager
    {
        private readonly string mailServer;
        private readonly string mailUser;
        private readonly string mailPassword;

        /// <summary>
        /// Gets or sets a value indicating whether mailing is suspended.
        /// </summary>
        public bool SuspendMailing { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailManager"/> class.
        /// </summary>
        /// <param name="server">Address of the mail server.</param>
        /// <param name="userName">Mail address of the user.</param>
        /// <param name="password">Mail password of the user.</param>
        public MailManager(string server, string userName, string password)
        {
            mailServer = server;
            mailUser = userName;
            mailPassword = password;
        }

        /// <summary>
        /// Sends mail to specified recipients.
        /// </summary>
        /// <param name="recipients">Recipients of the mail message.</param>
        /// <param name="subject">Subject of the mail message.</param>
        /// <param name="content">Content of the mail message.</param>
        public void SendMail(List<string> recipients, string subject, string content)
        {
            if (!recipients.Any())
            {
                return;
            }

            Send(PrepareMailMessage(recipients, subject, content));
        }
        
        /// <summary>
        /// Sends async mail to specified recipients.
        /// </summary>
        /// <param name="recipients">Recipients of the mail message.</param>
        /// <param name="subject">Subject of the mail message.</param>
        /// <param name="content">Content of the mail message.</param>
        /// <returns>Task which executes sending.</returns>
        public async Task SendMailAsync(List<string> recipients, string subject, string content)
        {
            await Task.Run(() => SendMail(recipients, subject, content));
        }

        /// <summary>
        /// Sends mail to specified recipients and carbon copy recipients.
        /// </summary>
        /// <param name="recipients">Recipients of the mail message.</param>
        /// <param name="ccRecipients">Carbon copy recipients of the mail message.</param>
        /// <param name="subject">Subject of the mail message.</param>
        /// <param name="content">Content of the mail message.</param>
        public void SendMail(List<string> recipients, List<string> ccRecipients, string subject, string content)
        {
            if (!recipients.Any() || !ccRecipients.Any())
            {
                return;
            }

            var mail = PrepareMailMessage(recipients, subject, content);

            foreach (var ccRecipient in ccRecipients)
            {
                mail.CC.Add(ccRecipient);
            }

            Send(mail);
        }

        /// <summary>
        /// Sends async mail to specified recipients and carbon copy recipients.
        /// </summary>
        /// <param name="recipients">Recipients of the mail message.</param>
        /// <param name="ccRecipients">Carbon copy recipients of the mail message.</param>
        /// <param name="subject">Subject of the mail message.</param>
        /// <param name="content">Content of the mail message.</param>
        /// <returns>Task which executes sending.</returns>
        public async Task SendMailAsync(List<string> recipients, List<string> ccRecipients, string subject, string content)
        {
            await Task.Run(() => SendMail(recipients, ccRecipients, subject, content));
        }

        /// <summary>
        /// Prepares a mail message for sending.
        /// </summary>
        /// <param name="recipients">Recipients of the message.</param>
        /// <param name="subject">Subject of the message.</param>
        /// <param name="body">Body of the message.</param>
        /// <returns>Object of type MailMessage.</returns>
        private MailMessage PrepareMailMessage(List<string> recipients, string subject, string body)
        {
            var mail = new MailMessage();

            foreach (var recipient in recipients)
            {
                mail.To.Add(recipient);
            }

            mail.From = new MailAddress(mailUser);
            mail.Subject = subject;
            mail.Body = body;

            return mail;
        }

        /// <summary>
        /// Sends passed mail message.
        /// </summary>
        /// <param name="mail">MailMessage to be send.</param>
        private void Send(MailMessage mail)
        {
            if (SuspendMailing)
            {
                return;
            }

            var smtp = new SmtpClient(mailServer);
            var credentials = new NetworkCredential(mailUser, mailPassword);

            smtp.Credentials = credentials;
            smtp.Send(mail);
        }
    }
}
