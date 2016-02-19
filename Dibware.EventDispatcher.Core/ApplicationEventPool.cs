using Dibware.EventDispatcher.Core.Contracts;
using System;
using System.Collections.Generic;

namespace Dibware.EventDispatcher.Core
{
    public class ApplicationEventPool : IApplicationEventPool
    {
        private readonly Dictionary<int, IPoolableApplicationEvent> _applicationEvents;

        public ApplicationEventPool()
        {
            _applicationEvents = new Dictionary<int, IPoolableApplicationEvent>();
        }

        public bool TryAdd<TEvent>(TEvent @event) where TEvent : class, IPoolableApplicationEvent
        {
            if (@event == null) throw new ArgumentNullException("event");

            if (_applicationEvents.ContainsKey(@event.HashCode)) return false;

            _applicationEvents.Add(@event.HashCode, @event);

            return true;
        }

        public bool TryGet<TEvent>(int hashCode, out TEvent @event) where TEvent : class, IPoolableApplicationEvent
        {
            IPoolableApplicationEvent applicationEvent;
            @event = null;

            if (_applicationEvents.TryGetValue(hashCode, out applicationEvent))
            {
                @event = applicationEvent as TEvent;
            }

            bool eventFound = (@event != null);
            return eventFound;
        }

        public bool TryRemove<TEvent>(out TEvent @event) where TEvent : class, IPoolableApplicationEvent
        {
            throw new NotImplementedException();

            //Type eventType = typeof(TEvent);
            //IApplicationEvent applicationEvent;
            //@event = null;

            //if (_applicationEvents.TryGetValue(eventType, out applicationEvent))
            //{
            //    @event = applicationEvent as TEvent;
            //    _applicationEvents.Remove(eventType);
            //}

            //bool eventFound = (@event != null);
            //return eventFound;
        }

        public void Clear()
        {
            _applicationEvents.Clear();
        }
    }
}