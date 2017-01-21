using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCore.Core.Application.Interfaces.Services;

namespace GameCore.Core.Services.Event
{
    public class EventService : IService
    {
        private Dictionary<Type, Dictionary<int, List<IEventListenerContainer>>> _eventListenerContainers = new Dictionary<Type, Dictionary<int, List<IEventListenerContainer>>>();
        public async Task Initialize()
        {
        }

        public void AddEventListener<TEventType,TEvent>(TEventType eventType, Action<TEvent> eventListener)
            where TEventType : struct
            where TEvent : BaseEvent<TEventType>
        {
            var invokationList = GetInvokationList(eventType);
            invokationList.Add(new EventListenerContainer<TEventType,TEvent>(eventListener));
        }

        public void RemoveEventListener<TEventType, TEvent>(TEventType eventType, Action<TEvent> eventListener)
            where TEventType : struct
            where TEvent : BaseEvent<TEventType>
        {
            var invokationList = GetInvokationList(eventType);
            invokationList.RemoveAll(e =>
                    e is EventListenerContainer<TEventType, TEvent> &&
                    ((EventListenerContainer<TEventType, TEvent>)e).EventListener == eventListener);
        }

        public bool HasEventListener<TEventType>(TEventType eventType)
            where TEventType : struct
        {
            if (_eventListenerContainers.ContainsKey(typeof(TEventType)))
            {
                return _eventListenerContainers[typeof(TEventType)].ContainsKey((int) (object) eventType);
            }
            return false;

        }

        public void DispatchEvent<TEventType, TEvent>(TEvent @event) 
            where TEventType : struct
            where TEvent : BaseEvent<TEventType>
        {
            if (HasEventListener(@event.EventType))
            {
                var invokationList = GetInvokationList(@event.EventType);
                for (int i = 0; i < invokationList.Count; i++)
                {
                    invokationList[i].InvokeEvent(@event);
                }
            }
        }
        
        public async Task Deinitialize()
        {

        }

        private List<IEventListenerContainer> GetInvokationList<TEventType>(TEventType eventType)
        {
            var eventTypeListeners = default(Dictionary<int, List<IEventListenerContainer>>);
            if (!_eventListenerContainers.TryGetValue(typeof(TEventType), out eventTypeListeners))
            {
                eventTypeListeners = new Dictionary<int, List<IEventListenerContainer>>();
                _eventListenerContainers.Add(typeof(TEventType), eventTypeListeners);
            }

            var invokationList = default(List<IEventListenerContainer>);
            if (!eventTypeListeners.TryGetValue((int)(object)eventType, out invokationList))
            {
                invokationList = new List<IEventListenerContainer>();
                eventTypeListeners.Add((int)(object)eventType, invokationList);
            }
            return invokationList;
        }
    }
}
