using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Silverpop.Core.XML
{
    public class LoginEncoder
    {
        public virtual string Encode(Login login)
        {
            if (login == null) throw new ArgumentNullException("login");

            var xml = new XElement(XName.Get("Envelope"));
            var bodyXml = new XElement(XName.Get("Body"));
            var loginXml = new XElement(XName.Get("Login"));

            loginXml.SetElementValue(XName.Get("USERNAME"), login.Username);
            loginXml.SetElementValue(XName.Get("PASSWORD"), login.Password);
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