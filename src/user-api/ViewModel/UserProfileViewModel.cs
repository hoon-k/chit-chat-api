namespace ChitChatAPI.UserAPI.ViewModel
{
    using System.Collections.Generic;

    public class UserProfileViewModel
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public UserProfileViewModel(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }
    }
}