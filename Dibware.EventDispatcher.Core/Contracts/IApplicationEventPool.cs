
namespace Dibware.EventDispatcher.Core.Contracts
{
    public interface IApplicationEventPool
    {
        bool TryAdd<TEvent>(TEvent @event) where TEvent : class, IApplicationEvent;
        bool TryGet<TEvent>(out TEvent @event) where TEvent : class, IApplicationEvent;
        bool TryRemove<TEvent>(out TEvent @event) where TEvent : class, IApplicationEvent;
        void Clear();
    }
}