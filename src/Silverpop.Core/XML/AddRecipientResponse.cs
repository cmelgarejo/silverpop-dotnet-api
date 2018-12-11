namespace Silverpop.Core.XML
{
    public class AddRecipientResponse
    {
        public AddRecipientResponse()
        {
        }

        public bool Success { get; set; }

        public string RawResponse { get; set; }

        public string RecipientId { get; set; }

        public string OrganizationId { get; set; }

        public string VisitorAssociation { get; set; }

        public string ErrorString { get; set; }
    }
}