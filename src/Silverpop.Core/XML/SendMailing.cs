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

        public static SendMailing Create(string mailingId, string recipientEmail)
        {
            if (mailingId == null) throw new ArgumentNullException("mailingId");
            if (recipientEmail == null) throw new ArgumentNullException("recipientEmail");

            return new SendMailing()
            {
                MailingId = mailingId,
                RecipientEmail = recipientEmail,
            };
        }
    }
}