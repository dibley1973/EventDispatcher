namespace Dibware.EventDispatcher.Core.Contracts
{
    public interface IPoolableApplicationEvent : IApplicationEvent
    {
        int Key { get; }
    }
}