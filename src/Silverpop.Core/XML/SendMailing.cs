using System;

namespace Silverpop.Core.XML
{
    public class SendMailing
    {
        public SendMailing()
        {

        }

        public string MailingId { get; set; }

        public string RecipientEmail { get; set; }

        /// <remarks>
        /// I'm not a huge fan of this manual cloning.
        /// However, I'm choosing this over taking a dependency on a mapper
        /// or performing a deep clone that includes the Recipients collection unnecessarily.
        /// </remarks>
        private static SendMailing CloneWithoutRecipients(SendMailing sendMailing)
        {
            return new SendMailing()
            {
                MailingId = sendMailing.MailingId,
                RecipientEmail = sendMailing.RecipientEmail
            };
        }

        public static SendMailing Create(string username, string password)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (password == null) throw new ArgumentNullException("password");

            return new SendMailing()
            {
                MailingId = username,
                RecipientEmail = password,
            };
        }
    }
}