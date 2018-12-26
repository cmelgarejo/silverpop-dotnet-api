using System.Collections.Generic;

namespace Silverpop.Core.XML
{
    public class SelectRecipientDataResponse
    {
        public SelectRecipientDataResponse()
        {
        }

        public bool Success { get; set; }

        public string RawResponse { get; set; }

        public string RecipientId { get; set; }

        public string OrganizationId { get; set; }

        public string Email { get; set; }

        public List<KeyValuePair<string,string>> Columns{ get; set; }

        public string ErrorString { get; set; }
    }
}