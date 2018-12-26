using System;
using System.Collections.Generic;

namespace Silverpop.Core.XML
{
    public class AddRecipient
    {
        public enum CreatedFromEnum
        {
            ImportedFromDatabase = 0,
            AddedManually = 1,
            OptedIn = 2,
            CreatedFromTrackingDatabase = 3
        }
        public AddRecipient()
        {
            Columns = new List<KeyValuePair<string, string>>();
            SyncFields = new List<KeyValuePair<string, string>>();
        }

        public string DatabaseId { get; set; }

        public int CreatedFrom { get; set; }

        public bool UpdateIfFound { get; set; }

        public ICollection<KeyValuePair<string, string>> Columns { get; set; }

        public ICollection<KeyValuePair<string, string>> SyncFields { get; set; }

        /// <remarks>
        /// I'm not a huge fan of this manual cloning.
        /// However, I'm choosing this over taking a dependency on a mapper
        /// or performing a deep clone that includes the Recipients collection unnecessarily.
        /// </remarks>
        private static AddRecipient CloneWithoutRecipients(AddRecipient contact)
        {
            return new AddRecipient()
            {
                DatabaseId = contact.DatabaseId,
                Columns = contact.Columns,
                UpdateIfFound = contact.UpdateIfFound,
                SyncFields = contact.SyncFields
            };
        }

        public static AddRecipient Create(string databaseId, CreatedFromEnum createdFrom = CreatedFromEnum.OptedIn, bool updateIfFound = false ,KeyValuePair<string, string>[] columns = null, params KeyValuePair<string, string>[] syncFields)
        {
            if (databaseId == null) throw new ArgumentNullException("databaseId");

            return new AddRecipient()
            {
                DatabaseId = databaseId,
                CreatedFrom = (int)createdFrom,
                Columns = columns,
                UpdateIfFound = updateIfFound,
                SyncFields = syncFields
            };
        }
    }
}