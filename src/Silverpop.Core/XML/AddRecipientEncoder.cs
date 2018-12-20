using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Silverpop.Core.XML
{
    public class AddRecipientEncoder
    {
        public virtual string Encode(AddRecipient message)
        {
            if (message == null) throw new ArgumentNullException("message");

            var xml = new XElement(XName.Get("Envelope"));
            var bodyXml = new XElement(XName.Get("Body"));
            var addReceipentXml = new XElement(XName.Get("AddRecipient"));

            addReceipentXml.SetElementValue(XName.Get("LIST_ID"), message.DatabaseId);
            addReceipentXml.SetElementValue(XName.Get("CREATED_FROM"), message.CreatedFrom);
            addReceipentXml.SetElementValue(XName.Get("UPDATE_IF_FOUND"), message.UpdateIfFound);
            // Add COLUMNS
            if (message.Columns.Count > 0)
            {
                foreach (var column in message.Columns)
                {
                    var columnXml = new XElement(XName.Get("COLUMN"));
                    var name = new XElement(XName.Get("NAME")) { Value = column.Key };
                    var value = new XElement(XName.Get("VALUE")) { Value = column.Value };

                    columnXml.Add(name);
                    columnXml.Add(value);

                    addReceipentXml.Add(columnXml);
                }
            }
            // Add SYNC_COLUMNS
            if (message.SyncFields != null)
            {
                var syncColumnsXml = new XElement(XName.Get("SYNC_FIELDS"));
                foreach (var syncColumn in message.SyncFields)
                {
                    var syncColumnXml = new XElement(XName.Get("SYNC_FIELD"));
                    var name = new XElement(XName.Get("NAME")) { Value = syncColumn.Key };
                    var value = new XElement(XName.Get("VALUE")) { Value = syncColumn.Value };

                    syncColumnXml.Add(name);
                    syncColumnXml.Add(value);

                    syncColumnsXml.Add(syncColumnXml);
                }
                addReceipentXml.Add(syncColumnsXml);
            }
            bodyXml.Add(addReceipentXml);
            xml.Add(bodyXml);

            return xml.ToString();
        }

        private static bool ContainsCDATASection(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;

            return Regex.Match(str, @"<!\[CDATA\[(.|\n|\r)*]]>").Success;
        }
    }
}