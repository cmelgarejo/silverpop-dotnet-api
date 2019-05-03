using System;
using System.Xml.Linq;

namespace Silverpop.Core.XML
{
    public class SendMailingResponseDecoder
    {
        public virtual SendMailingResponse Decode(string xmlResponse, string rawRequest = "")
        {
            if (xmlResponse == null) throw new ArgumentNullException("xmlResponse");

            var xml = XElement.Parse(xmlResponse);

            if (xml.Name != "Envelope")
                throw new ArgumentException("xmlResponse must have a root <Envelope> node.");

            var bodyXML = xml.Element(XName.Get("Body"));

            var resultXML = bodyXML.Element(XName.Get("RESULT"));

            bool success = Convert.ToBoolean(resultXML.Element(XName.Get("SUCCESS")).Value);
            string errorString = "", sessionId = "", sessionEncoding = "", organizationId = "";
            if (success)
            {
                sessionId = resultXML.Element(XName.Get("SESSIONID")).Value;
                sessionEncoding = resultXML.Element(XName.Get("SESSION_ENCODING")).Value;
                organizationId = resultXML.Element(XName.Get("ORGANIZATION_ID")).Value;
            }
            return new SendMailingResponse()
            {
                RawRequest = rawRequest,
                RawResponse = xmlResponse,
                Success = success,
                ErrorString = errorString,
                SessionId = sessionId,
                SessionEncoding = sessionEncoding,
                OrganizationId = organizationId
            };
        }
    }
}