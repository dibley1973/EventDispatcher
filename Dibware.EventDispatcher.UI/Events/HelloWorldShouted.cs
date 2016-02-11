using Dibware.EventDispatcher.Core.Contracts;

namespace Dibware.EventDispatcher.UI.Events
{
    internal class HelloWorldShouted : IApplicationEvent
    {
        public string Message
        {
            get { return "Hello, you event driven world, you!"; }
        }
    }
}