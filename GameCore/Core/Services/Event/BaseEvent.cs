namespace GameCore.Core.Services.Event
{
    public class BaseEvent
    {
        private int _eventType;
        public int EventType => _eventType;

        public BaseEvent(int eventType)
        {
            _eventType = eventType;
        }
    }

    public class BaseEvent<TEventType> : BaseEvent where TEventType : struct
    {
        public new TEventType EventType => (TEventType)(object)base.EventType;
        public BaseEvent(TEventType eventType) : base((int)(object)eventType)
        {
        }
    }
}