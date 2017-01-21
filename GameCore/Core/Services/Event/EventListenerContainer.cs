using System;
using GameCore.Core.Extentions;

namespace GameCore.Core.Services.Event
{
    public interface IEventListenerContainer
    {
        void InvokeEvent(BaseEvent @event);
    }

    public class EventListenerContainer<TEventType,TEvent> : IEventListenerContainer
        where TEventType : struct
        where TEvent : BaseEvent<TEventType>
    {
        public Action<TEvent> EventListener { get; }

        public EventListenerContainer(Action<TEvent> eventListener)
        {
            EventListener = eventListener;
        }
        public void InvokeEvent(BaseEvent @event)
        {
            EventListener.SafeInvoke((TEvent)@event,ActionInvokationType.MainThread);
        }
    }
}