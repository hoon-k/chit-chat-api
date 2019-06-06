using System;
using System.Threading.Tasks;
using ChitChatAPI.Common.Event;
using ChitChatAPI.Common.EventBus;

namespace ChitChatAPI.UserAPI.IntegrationsEvents.Services
{
    public class UserIntegrationEventService : IIntegrationEventService
    {
        private readonly IEventBus eventBus;

        public UserIntegrationEventService(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        public void PublishThroughEventBus(IntegrationEvent evt)
        {
            this.eventBus.Publish(evt);
        }
    }
}