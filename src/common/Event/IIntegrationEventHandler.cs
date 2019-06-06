using System.Threading.Tasks;

namespace ChitChatAPI.Common.Event
{
    public interface IIntegrationEventHandler<TEvent>
        where TEvent: IntegrationEvent
    {
        Task Handle(TEvent evt);
    }
}