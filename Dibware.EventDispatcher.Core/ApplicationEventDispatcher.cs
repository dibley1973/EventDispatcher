﻿using Dibware.EventDispatcher.Core.Contracts;
using System;
using System.Collections.Generic;

namespace Dibware.EventDispatcher.Core
{
    public class ApplicationEventDispatcher : IApplicationEventDispatcher
    {
        private bool _disposed;
        private Dictionary<Type, Delegate> _applicationEventHandlers;

        public ApplicationEventDispatcher()
        {
            _applicationEventHandlers = new Dictionary<Type, Delegate>();
        }

        ~ApplicationEventDispatcher()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // free other managed objects that implement IDisposable only
            }

            // release any unmanaged objects
            // set the object references to null
            RemoveAllListeners();

            _applicationEventHandlers = null;

            _disposed = true;
        }

        public void AddListener<TEvent>(ApplicationEventHandlerDelegate<TEvent> handler)
            where TEvent : IApplicationEvent
        {
            // TODO: Investigate; https://gist.github.com/stfx/3786466 tryGet

            if (_applicationEventHandlers.ContainsKey(typeof(TEvent)))
            {
                Delegate handlersForType = _applicationEventHandlers[typeof(TEvent)];
                _applicationEventHandlers[typeof(TEvent)] = Delegate.Combine(handlersForType, handler);
            }
            else
            {
                _applicationEventHandlers[typeof(TEvent)] = handler;
            }
        }

        public void RemoveListener<TEvent>(ApplicationEventHandlerDelegate<TEvent> handler)
            where TEvent : IApplicationEvent
        {
            if (_applicationEventHandlers.ContainsKey(typeof(TEvent)))
            {
                var handlerToRemove = Delegate.Remove(_applicationEventHandlers[typeof(TEvent)], handler);
                if (handlerToRemove == null)
                {
                    _applicationEventHandlers.Remove(typeof(TEvent));
                }
                else
                {
                    _applicationEventHandlers[typeof(TEvent)] = handlerToRemove;
                }
            }
        }

        public void Dispatch<TEvent>(TEvent @event) where TEvent : IApplicationEvent
        {
            if (@event == null)
            {
                throw new ArgumentNullException("event");
            }

            Delegate @delegate;
            if (_applicationEventHandlers.TryGetValue(typeof(TEvent), out @delegate))
            {
                ApplicationEventHandlerDelegate<TEvent> callback = @delegate as ApplicationEventHandlerDelegate<TEvent>;
                if (callback != null)
                {
                    callback(@event);
                }
            }
        }

        private void RemoveAllListeners()
        {
            var handlerTypes = new Type[_applicationEventHandlers.Keys.Count];
            _applicationEventHandlers.Keys.CopyTo(handlerTypes, 0);

            foreach (Type handlerType in handlerTypes)
            {
                Delegate[] delegates = _applicationEventHandlers[handlerType].GetInvocationList();
                foreach (Delegate @delegate1 in delegates)
                {
                    var handlerToRemove = Delegate.Remove(_applicationEventHandlers[handlerType], @delegate1);
                    if (handlerToRemove == null)
                    {
                        _applicationEventHandlers.Remove(handlerType);
                    }
                    else
                    {
                        _applicationEventHandlers[handlerType] = handlerToRemove;
                    }
                }
            }
        }
    }
}