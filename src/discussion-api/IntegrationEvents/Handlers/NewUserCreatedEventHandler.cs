using System;
using System.Threading.Tasks;
using ChitChatAPI.Common.Event;
using ChitChatAPI.Common.EventBus;
using ChitChatAPI.DiscussionAPI.IntegrationsEvents.Events;

namespace ChitChatAPI.DiscussionAPI.IntegrationsEvents.Handlers
{
    public class NewUsetCreatedEventHandler : IIntegrationEventHandler<NewUserCreatedEvent>
    {
        public async Task Handle(NewUserCreatedEvent evt)
        {
            var someTask = new Action(() => {
                Console.WriteLine("Handling NewUserCreatedEvent");
            });

            await Task.Run(someTask);
        }
    }
}