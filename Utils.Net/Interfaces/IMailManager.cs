using System.Collections.Generic;
using System.Threading.Tasks;

namespace Utils.Net.Interfaces
{
    /// <summary>
    /// Provides mail sending functionality.
    /// </summary>
    public interface IMailManager
    {
        /// <summary>
        /// Gets or sets a value indicating whether mailing is suspended.
        /// </summary>
        bool SuspendMailing { get; set; }


        /// <summary>
        /// Sends mail to specified recipients.
        /// </summary>
        /// <param name="recipients">Recipients of the mail message.</param>
        /// <param name="subject">Subject of the mail message.</param>
        /// <param name="content">Content of the mail message.</param>
        void SendMail(List<string> recipients, string subject, string content);

        /// <summary>
        /// Sends async mail to specified recipients.
        /// </summary>
        /// <param name="recipients">Recipients of the mail message.</param>
        /// <param name="subject">Subject of the mail message.</param>
        /// <param name="content">Content of the mail message.</param>
        /// <returns>Task which executes sending.</returns>
        Task SendMailAsync(List<string> recipients, string subject, string content);

        /// <summary>
        /// Sends mail to specified recipients and carbon copy recipients.
        /// </summary>
        /// <param name="recipients">Recipients of the mail message.</param>
        /// <param name="ccRecipients">Carbon copy recipients of the mail message.</param>
        /// <param name="subject">Subject of the mail message.</param>
        /// <param name="content">Content of the mail message.</param>
        void SendMail(List<string> recipients, List<string> ccRecipients, string subject, string content);

        /// <summary>
        /// Sends async mail to specified recipients and carbon copy recipients.
        /// </summary>
        /// <param name="recipients">Recipients of the mail message.</param>
        /// <param name="ccRecipients">Carbon copy recipients of the mail message.</param>
        /// <param name="subject">Subject of the mail message.</param>
        /// <param name="content">Content of the mail message.</param>
        /// <returns>Task which executes sending.</returns>
        Task SendMailAsync(List<string> recipients, List<string> ccRecipients, string subject, string content);
    }
}
