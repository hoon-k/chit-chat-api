namespace ChitChatAPI.UserAPI.DataModel
{
    using System.Collections.Generic;

    public class LoginRequest
    {
        public string Password { get; set; }

        public string Username { get; set; }
    }
}