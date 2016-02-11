using Dibware.EventDispatcher.Core.Tests.Fakes.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Dibware.EventDispatcher.Core.Tests
{
    [TestClass]
    public class ApplicationEventPoolTests
    {
        [TestMethod]
        public void GivenAnEventAndNoEventsExist_WhenTryingToAdd_ReturnsTrue()
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
        public void GivenAnEventOfDifferentTypeToAnyExisting_WhenTryingToAdd_ReturnsTrue()
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
        public void GivenAnEventOfSameTypeAsAlreadyExists_WhenTryingToAdd_ReturnsFalse()
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
        public void TryGet_WhenNoEventsAdded_ReturnsFalse()
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
        public void TryGet_WhenNoEventsAdded_OutputsNull()
        {
            // ARRANGE
            SimpleEvent1 event1;
            SimpleEvent1 actual;
            var eventPool = new ApplicationEventPool();

            // ACT
            eventPool.TryGet(out actual);

            // ASSERT
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void TryGet_WhenGettingDifferentEventTypeThanWhatWasAdded_ReturnsFalse()
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
        public void TryGet_WhenGettingDifferentEventTypeThanWhatWasAdded_OutputsNull()
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
        public void TryGet_WhenGettingSameEventTypeAsAdded_ReturnsTrue()
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
        public void TryGet_WhenGettingSameEventTypeAsAdded_OutputsInstance()
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
        public void TryGet_WhenGettingSameEventTypeAsAdded_OutputsSameInstance()
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
        public void TryRemove_WhenCalledForTypeAlreadyAdded_ReturnsTrue()
        {
            // ARRANGE
            var event1 = new SimpleEvent1();
            Type event1Type = event1.GetType();
            var eventPool = new ApplicationEventPool();
            eventPool.TryAdd(event1);

            // ACT
            var actual = eventPool.TryRemove(event1Type);

            // ASSERT
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TryRemove_WhenCalledForTypeNotAdded_ReturnsFalse()
        {
            // ARRANGE
            var event1 = new SimpleEvent1();
            Type event1Type = event1.GetType();
            var eventPool = new ApplicationEventPool();
            eventPool.TryAdd(event1);

            // ACT
            var actual = eventPool.TryRemove(typeof(SimpleEvent2));

            // ASSERT
            Assert.IsFalse(actual);
        }
    }
}