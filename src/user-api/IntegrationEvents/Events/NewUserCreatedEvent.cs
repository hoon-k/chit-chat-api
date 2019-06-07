using System;
using System.Threading.Tasks;
using ChitChatAPI.Common.Event;
using ChitChatAPI.Common.EventBus;

namespace ChitChatAPI.UserAPI.IntegrationsEvents.Events
{
    public class NewUserCreatedEvent : IntegrationEvent
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string UUID { get; private set; }
        public string ScreenName { get; private set; }
        public string Role { get; private set; }

        public NewUserCreatedEvent(string firstName, string lastName, string uuid, string screenName, string role)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.UUID = uuid;
            this.ScreenName = screenName;
            this.Role = role;
        }
    }
}