
namespace Dibware.EventDispatcher.Core.Contracts
{
    public interface IApplicationEventPool
    {
        bool TryAdd<TEvent>(TEvent @event) where TEvent : class, IPoolableApplicationEvent;
        bool TryGet<TEvent>(int hashCode, out TEvent @event) where TEvent : class, IPoolableApplicationEvent;
        bool TryRemove<TEvent>(out TEvent @event) where TEvent : class, IPoolableApplicationEvent;
        void Clear();
    }
}