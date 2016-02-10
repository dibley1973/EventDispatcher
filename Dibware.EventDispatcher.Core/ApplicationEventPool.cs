using Dibware.EventDispatcher.Core.Contracts;
using System;
using System.Collections.Generic;

namespace Dibware.EventDispatcher.Core
{
    public class ApplicationEventPool : IApplicationEventPool
    {
        private readonly Dictionary<Type, IApplicationEvent> _applicationEvents;

        public ApplicationEventPool()
        {
            _applicationEvents = new Dictionary<Type, IApplicationEvent>();
        }

        public bool TryAdd<TEvent>(TEvent @event) where TEvent : class, IApplicationEvent
        {
            if (@event == null) throw new ArgumentNullException("event");

            Type eventType = typeof(TEvent);
            if (_applicationEvents.ContainsKey(eventType)) return false;

            _applicationEvents.Add(eventType, @event);

            return true;
        }

        public bool TryGet<TEvent>(out TEvent @event) where TEvent : class, IApplicationEvent
        {
            Type eventType = typeof(TEvent);
            IApplicationEvent applicationEvent;
            @event = null;

            if (_applicationEvents.TryGetValue(eventType, out applicationEvent))
            {
                @event = applicationEvent as TEvent;
            }

            bool eventFound = (@event != null);
            return eventFound;
        }

        public bool TryRemove<TEvent>(Type eventType) where TEvent : class, IApplicationEvent
        {
            if (!_applicationEvents.ContainsKey(eventType)) return false;

            _applicationEvents.Remove(eventType);
            return true;
        }

        public void Clear()
        {
            _applicationEvents.Clear();
        }
    }
}