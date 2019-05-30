namespace ChitChatAPI.UserAPI.ViewModel
{
    using System.Collections.Generic;

    public class UserProfileViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public UserProfileViewModel() { }

        public UserProfileViewModel(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }
    }
}