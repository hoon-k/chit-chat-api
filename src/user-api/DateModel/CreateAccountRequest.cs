namespace ChitChatAPI.UserAPI.DataModel
{
    using System.Collections.Generic;

    public class CreateAccountRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }
    }
}