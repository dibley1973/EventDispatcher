# Centralised Event Dispatcher in C# - Part 3
This is part three of a series of posts where I am going to share with you the solution I have used in a recent project for a centralised event dispatcher. All of the source code will be available in my public GitHub repositories. The concept I have tried to realise is a central class that can raise events for any listener to subscribe to. 

In [part one](http://www.duanewingett.info/2016/02/10/CentralisedEventDispatcherInCPart1.aspx) we created the event dispatcher class library, in [part two](http://www.duanewingett.info/2016/02/10/CentralisedEventDispatcherInCPart1.aspx) we consumed it from a windows forms application. In this part we are going to look at pooling commonly raised events.

## Assumptions
It is assumed that the reader of this article already has a good understanding of coding with C#, can create projects and solutions, classes and Windows Forms, and can add references to a project and import references into a class. It is also assumed the reader understands basic inheritance and interface implementation.

So one of the issues with the solution we currently have is as the system grows the number of events being raised may grow astronomically, and if each event raised creates a new instance then a memory concious system may start to "sweat" especially if the event data is quite large.

With this in mind we can look to minimise this slightly by allowing the system to pool commonly used events. 

### Dibware.EventDispatcher.Core

So lets move back into the core class library and see if we can implement a system to pool some of the commonly used events.

First lets define the contract which we think we are going to need for an object that pools Application Events. In the `Contracts` folder we will create a new interface named `IApplicationEventPool`. We probably need a method to try to add an event, one to try and get an event, one to try and remove and event, and a last to remove all events.

public interface IApplicationEventPool
{
    bool TryAdd<TEvent>(TEvent @event) where TEvent : class, IApplicationEvent;
    bool TryGet<TEvent>(out TEvent @event) where TEvent : class, IApplicationEvent;
    bool TryRemove(Type eventType);
    void Clear();
}

For a class which implements this contract:
* `TryAdd` should try and add an event to the backing store of the class if an event of that specified type does not exist.
* `TryGet` should retrieve an event of the specified type if one exists in the backing store
* `TryRemove` should remove an event of the specified type from the backing store if it exists.
* `RemoveAll` should remove all events from the backings store

So now we can create the implementation of the interface as the `ApplicationEventPool` class. This will require a backing store which will be a dictionary with the key which will be a `System.Type` and a value which will be of `IApplicationEvent`

public class ApplicationEventPool : IApplicationEventPool
    {
        private readonly Dictionary<Type, IApplicationEvent> _applicationEvents;

        public ApplicationEventPool()
        {
            _applicationEvents = new Dictionary<Type, IApplicationEvent>();
        }

        public bool TryAdd<TEvent>(TEvent @event) where TEvent : class, IApplicationEvent
        {
            if (@event == null) throw new ArgumentNullException("event");

            Type eventType = typeof(TEvent);
            if (_applicationEvents.ContainsKey(eventType)) return false;

            _applicationEvents.Add(eventType, @event);

            return true;
        }

        public bool TryGet<TEvent>(out TEvent @event) where TEvent : class, IApplicationEvent
        {
            Type eventType = typeof(TEvent);
            IApplicationEvent applicationEvent;
            @event = null;

            if (_applicationEvents.TryGetValue(eventType, out applicationEvent))
            {
                @event = applicationEvent as TEvent;
            }

            bool eventFound = (@event != null);
            return eventFound;
        }

        public bool TryRemove(Type eventType) //where TEvent : class, IApplicationEvent
        {
            if (!_applicationEvents.ContainsKey(eventType)) return false;

            _applicationEvents.Remove(eventType);
            return true;
        }

        public void Clear()
        {
            _applicationEvents.Clear();
        }
    }

A set of units tests for this class can be found in the `ApplicationEventPoolTests` of the `Dibware.EventDispatcher.Core.Tests` in teh repository.


## Disclaimer - Memory Usage
I am not an expert on Garbage Collection, so please be aware that pooling may not be the best solution for a memory problem using this Event Dispatcher. Pooling may lead to objects that should be short lived and would normally live in Generation 0 and get reclaimed quickly, being moved to Generation 1 or 2 and maybe not being reclaimed until the process ends. You can use `PerfMon.exe` or other profiling tools to help identify memory issues, and make an informed decision on how to combat them.

For further information on the Garbage Collector and [Garbage Collector Generations](https://msdn.microsoft.com/en-us/library/ee787088(v=vs.110).aspx#generations) please see [Fundamentals of Garbage Collection](https://msdn.microsoft.com/en-us/library/ee787088(v=vs.110).aspx)