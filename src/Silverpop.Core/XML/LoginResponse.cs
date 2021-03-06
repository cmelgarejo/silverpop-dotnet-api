﻿namespace Silverpop.Core.XML
{
    public class LoginResponse
    {
        public LoginResponse()
        {
        }

        public bool Success { get; set; }

        public string RawResponse { get; set; }

        public string SessionId { get; set; }

        public string SessionEncoding { get; set; }

        public string OrganizationId { get; set; }

        public string ErrorString { get; set; }
    }
}