using System;
using System.Collections.Generic;

namespace Silverpop.Core.XML
{
    public class SelectRecipientData
    {
        public SelectRecipientData()
        {
            Columns = new List<KeyValuePair<string, string>>();
        }

        public string Email { get; set; }

        public string DatabaseId { get; set; }

        public ICollection<KeyValuePair<string, string>> Columns { get; set; }

        /// <remarks>
        /// I'm not a huge fan of this manual cloning.
        /// However, I'm choosing this over taking a dependency on a mapper
        /// or performing a deep clone that includes the Recipients collection unnecessarily.
        /// </remarks>
        private static SelectRecipientData CloneWithoutRecipients(SelectRecipientData contact)
        {
            return new SelectRecipientData()
            {
                DatabaseId = contact.DatabaseId,
                Columns = contact.Columns,
            };
        }

        public static SelectRecipientData Create(string databaseId, string email, params KeyValuePair<string, string>[] columns)
        {
            if (databaseId == null) throw new ArgumentNullException("databaseId");
            if (email == null) throw new ArgumentNullException("email");

            return new SelectRecipientData()
            {
                DatabaseId = databaseId,
                Email = email,
                Columns = columns,
            };
        }
    }
}