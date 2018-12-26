using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Silverpop.Core.XML
{
    public class SelectRecipientDataResponseDecoder
    {
        public virtual SelectRecipientDataResponse Decode(string xmlResponse)
        {
            if (xmlResponse == null) throw new ArgumentNullException("xmlResponse");

            var xml = XElement.Parse(xmlResponse);

            if (xml.Name != "Envelope")
                throw new ArgumentException("xmlResponse must have a root <Envelope> node.");

            var bodyXML = xml.Element(XName.Get("Body"));

            bool success = Convert.ToBoolean(bodyXML.Element(XName.Get("RESULT")).Element(XName.Get("SUCCESS")).Value);
            var error = bodyXML.Element(XName.Get("Fault"));
            string errorString = "", email = "", recipientId = "", organizationId = "";
            List<KeyValuePair<string, string>> parsedCols = new List<KeyValuePair<string, string>>();
            if (error != null)
                errorString = error.Element(XName.Get("FaultString")).Value;

            else
            {
                var resultBody = bodyXML.Element(XName.Get("RESULT"));

                email = resultBody.Element(XName.Get("EMAIL")).Value;
                recipientId = resultBody.Element(XName.Get("RecipientId")).Value;
                organizationId = resultBody.Element(XName.Get("ORGANIZATION_ID")).Value;
                var columns = resultBody.Element(XName.Get("COLUMNS")).Nodes();
                foreach (var column in columns)
                {
                    try
                    {
                        var colName = (column as XElement).Element("NAME").Value;
                        var colValue = (column as XElement).Element("VALUE").Value;
                        parsedCols.Add(new KeyValuePair<string, string>(colName, colValue));
                    }
                    catch
                    {
                        // Do Nothing ? 
                    }
                }
                //visitorAssociation = bodyXML.Element(XName.Get("RESULT")).Element(XName.Get("VISITOR_ASSOCIATION")).Value;
            }
            return new SelectRecipientDataResponse()
            {
                RawResponse = xmlResponse,
                Success = success,
                ErrorString = errorString,
                RecipientId = recipientId,
                OrganizationId = organizationId,
                Email = email,
                Columns = parsedCols
            };
        }
    }
}