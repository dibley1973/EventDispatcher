using Dibware.EventDispatcher.Core.Tests.Fakes.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Dibware.EventDispatcher.Core.Contracts;

namespace Dibware.EventDispatcher.Core.Tests
{
    [TestClass]
    public class ApplicationEventDispatcherTests
    {
        [TestMethod]
        public void GivenAnEventToDispatch_WhenAHandlerIsNotAttached_DoesNotThrowAnException()
        {
            // ARRANGE
            var dispatcher = new ApplicationEventDispatcher();

            // ACT
            dispatcher.Dispatch(new SimpleEvent1());
            dispatcher.Dispose();

            // ASSERT
        }

        [TestMethod]
        public void GivenAnEventToDispatch_WhenAHandlerIsAttached_CallsHandler()
        {
            // ARRANGE
            bool handlerCalled = false;
            ApplicationEventHandlerDelegate<SimpleEvent1> @delegate = 
                delegate { 
                    handlerCalled = true; 
                };
            var dispatcher = new ApplicationEventDispatcher();
            dispatcher.AddListener(@delegate);

            // ACT
            dispatcher.Dispatch(new SimpleEvent1());
            dispatcher.Dispose();

            // ASSERT
            Assert.IsTrue(handlerCalled);
        }

        [TestMethod]
        public void GivenAnEventToDispatch_WhenAHandlerIsAttachedAndDetached_DoesNotCallHandler()
        {
            // ARRANGE
            bool handlerCalled = false;
            ApplicationEventHandlerDelegate<SimpleEvent1> @delegate =
                delegate {
                    handlerCalled = true; 
                };
            var dispatcher = new ApplicationEventDispatcher();
            dispatcher.AddListener(@delegate);
            dispatcher.RemoveListener(@delegate);
            
            // ACT
            dispatcher.Dispatch(new SimpleEvent1());
            dispatcher.Dispose();

            // ASSERT
            Assert.IsFalse(handlerCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void GivenAnEventToDispatch_WhenDispatcherIsDisposed_ThrowsException()
        {
            // ARRANGE
            var dispatcher = new ApplicationEventDispatcher();

            // ACT
            dispatcher.Dispose();
            dispatcher.Dispatch(new SimpleEvent1());

            // ASSERT
        }
    }
}
