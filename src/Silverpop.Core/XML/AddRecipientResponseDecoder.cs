using System;
using System.Xml.Linq;

namespace Silverpop.Core.XML
{
    public class AddRecipientResponseDecoder
    {
        public virtual AddRecipientResponse Decode(string xmlResponse)
        {
            if (xmlResponse == null) throw new ArgumentNullException("xmlResponse");

            var xml = XElement.Parse(xmlResponse);

            if (xml.Name != "Envelope")
                throw new ArgumentException("xmlResponse must have a root <Envelope> node.");

            var bodyXML = xml.Element(XName.Get("Body"));

            bool success = Convert.ToBoolean(bodyXML.Element(XName.Get("RESULT")).Element(XName.Get("SUCCESS")).Value);
            var error = bodyXML.Element(XName.Get("Fault"));
            string errorString = "", recipientId = "", organizationId = "", visitorAssociation = "";
            if (error != null)
                errorString = error.Element(XName.Get("FaultString")).Value;

            else
            {
                recipientId = bodyXML.Element(XName.Get("RESULT")).Element(XName.Get("RecipientId")).Value;
                organizationId = bodyXML.Element(XName.Get("RESULT")).Element(XName.Get("ORGANIZATION_ID")).Value;
                //visitorAssociation = bodyXML.Element(XName.Get("RESULT")).Element(XName.Get("VISITOR_ASSOCIATION")).Value;
            }
            return new AddRecipientResponse()
            {
                RawResponse = xmlResponse,
                Success = success,
                ErrorString = errorString,
                RecipientId = recipientId,
                OrganizationId = organizationId,
                VisitorAssociation = visitorAssociation
            };
        }
    }
}