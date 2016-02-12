using Dibware.EventDispatcher.Core.Tests.Fakes.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dibware.EventDispatcher.Core.Tests.Tests
{
    [TestClass]
    public class ApplicationEventPoolTests
    {
        [TestMethod]
        public void GivenAnEventAndNoEventsExist_WhenTryingToAdd_TrueIsReturned()
        {
            // ARRANGE
            var event1 = new SimpleEvent1();
            var eventPool = new ApplicationEventPool();

            // ACT
            var actual = eventPool.TryAdd(event1);

            // ASSERT
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void GivenAnEventOfDifferentTypeToAnyExisting_WhenTryingToAdd_TrueIsReturned()
        {
            // ARRANGE
            var event1 = new SimpleEvent1();
            var event2 = new SimpleEvent2();
            var eventPool = new ApplicationEventPool();
            eventPool.TryAdd(event1);

            // ACT
            var actual = eventPool.TryAdd(event2);

            // ASSERT
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void GivenAnEventOfSameTypeAsAlreadyExists_WhenTryingToAdd_FalseIsReturned()
        {
            // ARRANGE
            var event1 = new SimpleEvent1();
            var event2 = new SimpleEvent1();
            var eventPool = new ApplicationEventPool();
            eventPool.TryAdd(event1);

            // ACT
            var actual = eventPool.TryAdd(event2);

            // ASSERT
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void GivenAnEventOfSameTypeAndSameDataTypeAsAlreadyExists_WhenTryingToAdd_FalseIsReturned()
        {
            // ARRANGE
            var event1 = new EventWithSimpleData("Hello");
            var event2 = new EventWithSimpleData("Hello");
            var eventPool = new ApplicationEventPool();
            eventPool.TryAdd(event1);

            // ACT
            var actual = eventPool.TryAdd(event2);

            // ASSERT
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void GivenAnEventOfSameTypeButDifferentDataTypeAsAlreadyExists_WhenTryingToAdd_TrueIsReturned()
        {
            // ARRANGE
            var event1 = new EventWithSimpleData("Hello");
            var event2 = new EventWithSimpleData("world!");
            var eventPool = new ApplicationEventPool();
            eventPool.TryAdd(event1);

            // ACT
            var actual = eventPool.TryAdd(event2);

            // ASSERT
            Assert.IsTrue(actual);
        }


        [TestMethod]
        public void GivenNoEventsHaveBeenAdded_WhenTryingToGetEvent_FalseIsReturned()
        {
            // ARRANGE
            SimpleEvent1 event1;
            var eventPool = new ApplicationEventPool();

            // ACT
            var actual = eventPool.TryGet(out event1);

            // ASSERT
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void GivenNoEventsHaveBeenAdded_WhenTryingToGetEvent_OutputIsNull()
        {
            // ARRANGE
            SimpleEvent1 actual;
            var eventPool = new ApplicationEventPool();

            // ACT
            eventPool.TryGet(out actual);

            // ASSERT
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void GivenADifferentEventFromWhatHasBeenAdded_WhenTryingToGet_FalseIsReturned()
        {
            // ARRANGE
            var event1 = new SimpleEvent1();
            SimpleEvent2 event2;
            var eventPool = new ApplicationEventPool();
            eventPool.TryAdd(event1);

            // ACT
            var actual = eventPool.TryGet(out event2);

            // ASSERT
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void GivenADifferentEventFromWhatHasBeenAdded_WhenTryingToGet_OutputIsNull()
        {
            // ARRANGE
            var event1 = new SimpleEvent1();
            SimpleEvent2 actual;
            var eventPool = new ApplicationEventPool();
            eventPool.TryAdd(event1);

            // ACT
            eventPool.TryGet(out actual);

            // ASSERT
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void GivenSameEventTypeAsAdded_WhenTryingToGet_TrueIsReturned()
        {
            // ARRANGE
            var event1 = new SimpleEvent1();
            SimpleEvent1 event2;
            var eventPool = new ApplicationEventPool();
            eventPool.TryAdd(event1);

            // ACT
            var actual = eventPool.TryGet(out event2);

            // ASSERT
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void GivenSameEventTypeAsAdded_WhenTryingToGet_OutputIsInstanceOfEvent()
        {
            // ARRANGE
            var event1 = new SimpleEvent1();
            SimpleEvent1 actual;
            var eventPool = new ApplicationEventPool();
            eventPool.TryAdd(event1);

            // ACT
            eventPool.TryGet(out actual);

            // ASSERT
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void GivenSameEventTypeAsAdded_WhenTryingToGet_OutputIsSameInstance()
        {
            // ARRANGE
            var event1 = new SimpleEvent1();
            SimpleEvent1 actual;
            var eventPool = new ApplicationEventPool();
            eventPool.TryAdd(event1);

            // ACT
            eventPool.TryGet(out actual);

            // ASSERT
            Assert.AreSame(actual, event1);
        }

        [TestMethod]
        public void GivenSameEventTypeAsAdded_WhenTryingToRemove_TrueIsReturned()
        {
            // ARRANGE
            var event1 = new SimpleEvent1();
            var eventPool = new ApplicationEventPool();
            eventPool.TryAdd(event1);

            // ACT
            var actual = eventPool.TryRemove(out event1);

            // ASSERT
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void GivenSameEventTypeAsAdded_WhenTryingToRemove_OutputIssameInstanceAsImput()
        {
            // ARRANGE
            var event1 = new SimpleEvent1();
            SimpleEvent1 event2;
            var eventPool = new ApplicationEventPool();
            eventPool.TryAdd(event1);

            // ACT
            var actual = eventPool.TryRemove(out event2);

            // ASSERT
            Assert.AreSame(event1, event2);
        }

        [TestMethod]
        public void GivenADifferentEventFromWhatHasBeenAdded_WhenTryingToRemove_FalseIsReturned()
        {
            // ARRANGE
            var event1 = new SimpleEvent1();
            SimpleEvent2 event2;
            var eventPool = new ApplicationEventPool();
            eventPool.TryAdd(event1);

            // ACT
            var actual = eventPool.TryRemove(out event2);

            // ASSERT
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void GivenADifferentEventFromWhatHasBeenAdded_WhenTryingToRemove_OutputIsNull()
        {
            // ARRANGE
            var event1 = new SimpleEvent1();
            SimpleEvent2 event2;
            var eventPool = new ApplicationEventPool();
            eventPool.TryAdd(event1);

            // ACT
            var actual = eventPool.TryRemove(out event2);

            // ASSERT
            Assert.IsNull(event2);
        }
    }
}