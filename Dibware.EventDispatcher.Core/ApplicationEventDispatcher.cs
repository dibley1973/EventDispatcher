using Dibware.EventDispatcher.Core.Contracts;
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


        public void Raise<T>(T @event) where T : IApplicationEvent
        {
            if (@event == null)
            {
                throw new ArgumentNullException("event");
            }

            Delegate @delegate;
            if (_applicationEventHandlers.TryGetValue(typeof(T), out @delegate))
            {
                ApplicationEventHandlerDelegate<T> callback = @delegate as ApplicationEventHandlerDelegate<T>;
                if (callback != null)
                {
                    callback(@event);
                }
            }
        }

        public void Dispatch(IApplicationEvent @event)
        {
            if (@event == null)
            {
                return;
            }

            if (_applicationEventHandlers.ContainsKey(@event.GetType()))
            {
                _applicationEventHandlers[@event.GetType()].DynamicInvoke(@event);
            }
        }

        private void RemoveAllListeners()
        {
            foreach (Type handlerType in _applicationEventHandlers.Keys)
            {
                Delegate[] delegates = _applicationEventHandlers[handlerType].GetInvocationList();
                foreach (Delegate @delegate in delegates)
                {
                    dynamic applicationEventHandler = Convert.ChangeType(@delegate, handlerType);
                    _applicationEventHandlers[handlerType] -= applicationEventHandler;
                }
            }

            //Delegate[] delegates2 = _applicationEventHandlers[handlerType].GetInvocationList();
            //foreach (Delegate @delegate1 in delegates)
            //{
            //    var handlerToRemove = Delegate.Remove(_applicationEventHandlers[handlerType], @delegate1);
            //    if (handlerToRemove == null)
            //    {
            //        _applicationEventHandlers.Remove(handlerType);
            //    }
            //    else
            //    {
            //        _applicationEventHandlers[handlerType] = handlerToRemove;
            //    }
            //}


            //while (delegates2.Length > 0)
            //{
            //    var handlerToRemove = Delegate.Remove(_applicationEventHandlers[handlerType, handler);
            //    if (handlerToRemove == null)
            //    {
            //        _applicationEventHandlers.Remove(typeof(TEvent));
            //    }
            //    else
            //    {
            //        _applicationEventHandlers[typeof(TEvent)] = handlerToRemove;
            //    }
            //}
        }
    }
}