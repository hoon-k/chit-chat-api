using System.Threading.Tasks;

namespace ChitChatAPI.Common.Event
{
    public interface IIntegrationEventService
    {
        void PublishThroughEventBus(IntegrationEvent evt);
    }
}