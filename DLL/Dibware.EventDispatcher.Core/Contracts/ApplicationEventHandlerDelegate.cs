namespace Dibware.EventDispatcher.Core.Contracts
{
    public delegate void ApplicationEventHandlerDelegate<in TEvent>(TEvent @event) where TEvent : IApplicationEvent;
}