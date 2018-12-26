﻿using Silverpop.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Silverpop.Core
{
    public class TransactMessage
    {
        public TransactMessage()
        {
            SaveColumns = new List<string>();
            Recipients = new List<TransactMessageRecipient>();
        }

        public string CampaignId { get; set; }

        public string TransactionId { get; set; }

        public bool ShowAllSendDetail { get; set; }

        public bool SendAsBatch { get; set; }

        public bool NoRetryOnFailure { get; set; }

        public ICollection<string> SaveColumns { get; set; }

        public ICollection<TransactMessageRecipient> Recipients { get; set; }

        public IEnumerable<TransactMessage> SelectRecipientDataBatchedMessages(int maxRecipientsPerMessage)
        {
            if (maxRecipientsPerMessage <= 0)
                throw new ArgumentOutOfRangeException("maxRecipientsPerMessage");

            var messages = new List<TransactMessage>();
            foreach (var recipientsBatch in this.Recipients.Batch(maxRecipientsPerMessage))
            {
                var batchMessage = CloneWithoutRecipients(this);
                batchMessage.Recipients = recipientsBatch.ToList();

                messages.Add(batchMessage);
            }

            return messages;
        }

        /// <remarks>
        /// I'm not a huge fan of this manual cloning.
        /// However, I'm choosing this over taking a dependency on a mapper
        /// or performing a deep clone that includes the Recipients collection unnecessarily.
        /// </remarks>
        private static TransactMessage CloneWithoutRecipients(TransactMessage message)
        {
            return new TransactMessage()
            {
                CampaignId = message.CampaignId,
                TransactionId = message.TransactionId,
                ShowAllSendDetail = message.ShowAllSendDetail,
                SendAsBatch = message.SendAsBatch,
                NoRetryOnFailure = message.NoRetryOnFailure,
                SaveColumns = message.SaveColumns,
            };
        }

        public static TransactMessage Create(string campaignId, params TransactMessageRecipient[] recipients)
        {
            if (campaignId == null) throw new ArgumentNullException("campaignId");
            if (recipients == null) throw new ArgumentNullException("recipients");

            return new TransactMessage()
            {
                CampaignId = campaignId,
                Recipients = recipients
            };
        }
    }
}