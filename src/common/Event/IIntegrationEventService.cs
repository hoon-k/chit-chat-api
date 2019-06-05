using System.Threading.Tasks;

namespace ChitChatAPI.Common.Event
{
    public interface IIntegrationServiceEvent
    {
        void PublishThroughEventBus(IntegrationEvent evt);
    }
}