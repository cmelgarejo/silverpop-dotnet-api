using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Silverpop.Core.XML
{
    public class SendMailingEncoder
    {
        public virtual string Encode(SendMailing sendMailing)
        {
            if (sendMailing == null) throw new ArgumentNullException("sendMailing");

            var xml = new XElement(XName.Get("Envelope"));
            var bodyXml = new XElement(XName.Get("Body"));
            var loginXml = new XElement(XName.Get("SendMailing"));

            loginXml.SetElementValue(XName.Get("MailingId"), sendMailing.MailingId);
            loginXml.SetElementValue(XName.Get("RecipientEmail"), sendMailing.RecipientEmail);
            bodyXml.Add(loginXml);
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