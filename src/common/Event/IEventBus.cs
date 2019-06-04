using System;

namespace ChitChatAPI.Common.Event
{
    public interface IEventBus
    {
        void Publish();
        void Subscribe();
        void Unsubscribe();
    }
}