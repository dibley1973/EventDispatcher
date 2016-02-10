using System;

namespace Dibware.EventDispatcher.Core.Contracts
{
    public interface IApplicationEventDispatcher : IDisposable
    {
        void AddListener<TEvent>(ApplicationEventHandlerDelegate<TEvent> handler) where TEvent : IApplicationEvent;
        void RemoveListener<TEvent>(ApplicationEventHandlerDelegate<TEvent> handler) where TEvent : IApplicationEvent;
        void Dispatch<TEvent>(TEvent @event) where TEvent : IApplicationEvent;
    }
}