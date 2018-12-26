using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Silverpop.Core.XML
{
    public class SelectRecipientDataEncoder
    {
        public virtual string Encode(SelectRecipientData message)
        {
            if (message == null) throw new ArgumentNullException("message");

            var xml = new XElement(XName.Get("Envelope"));
            var bodyXml = new XElement(XName.Get("Body"));
            var addReceipentXml = new XElement(XName.Get("SelectRecipientData"));

            addReceipentXml.SetElementValue(XName.Get("LIST_ID"), message.DatabaseId);
            addReceipentXml.SetElementValue(XName.Get("EMAIL"), message.Email);
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