using System;

namespace Silverpop.Core.XML
{
    public class Login
    {
        public Login()
        {

        }

        public string Username { get; set; }

        public string Password { get; set; }

        /// <remarks>
        /// I'm not a huge fan of this manual cloning.
        /// However, I'm choosing this over taking a dependency on a mapper
        /// or performing a deep clone that includes the Recipients collection unnecessarily.
        /// </remarks>
        private static Login CloneWithoutRecipients(Login login)
        {
            return new Login()
            {
                Username = login.Username,
                Password = login.Password
            };
        }

        public static Login Create(string username, string password)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (password == null) throw new ArgumentNullException("password");

            return new Login()
            {
                Username = username,
                Password = password,
            };
        }
    }
}