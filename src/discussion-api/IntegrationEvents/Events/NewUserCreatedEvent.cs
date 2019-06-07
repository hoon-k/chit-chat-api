using System;
using System.Threading.Tasks;
using ChitChatAPI.Common.Event;
using ChitChatAPI.Common.EventBus;

namespace ChitChatAPI.DiscussionAPI.IntegrationsEvents.Events
{
    public class NewUserCreatedEvent : IntegrationEvent
    {
        public string ScreenName { get; private set; }
        public string Role { get; private set; }

        public NewUserCreatedEvent(string firstName, string lastName, string screenName, string role)
        {
            this.ScreenName = screenName;
            this.Role = role;
        }
    }
}