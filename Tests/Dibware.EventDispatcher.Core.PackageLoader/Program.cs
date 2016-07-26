using Dibware.EventDispatcher.Core.Contracts;
using Dibware.EventDispatcher.Core.PackageLoader.Events;
using System;

namespace Dibware.EventDispatcher.Core.PackageLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            // ARRANGE
            bool handlerCalled = false;
            ApplicationEventHandlerDelegate<SimpleEvent1> @delegate =
                delegate
                {
                    handlerCalled = true;
                };
            var dispatcher = new ApplicationEventDispatcher();
            dispatcher.AddListener(@delegate);

            // ACT
            dispatcher.Dispatch(new SimpleEvent1());
            dispatcher.Dispose();

            // ASSERT
            Console.WriteLine("Handler called: {0}", handlerCalled);

            Console.ReadKey();
        }
    }
}
