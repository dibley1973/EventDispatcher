# Centralised Event Dispatcher in C# - Part 3
This is part three of a series of posts where I am going to share with you the solution I have used in a recent project for a centralised event dispatcher. All of the source code will be available in my public GitHub repositories. The concept I have tried to realise is a central class that can raise events for any listener to subscribe to. 

In [part one](http://www.duanewingett.info/2016/02/10/CentralisedEventDispatcherInCPart1.aspx) we created the event dispatcher class library, in [part two](http://www.duanewingett.info/2016/02/10/CentralisedEventDispatcherInCPart1.aspx) we consumed it from a windows forms application. In this part we are going to look at pooling commonly raised events and implement an 'ApplicationEventPool'.

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
    bool TryRemove<TEvent>(out TEvent @event) where TEvent : class, IApplicationEvent;
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

PLEASE NOTE: A set of units tests for the above class can be found in the `ApplicationEventPoolTests` class of the `Dibware.EventDispatcher.Core.Tests` in the repository.

Now we have an ApplicationEventPoolObject lets switch back to the UI project and set up a scenario which could use it.

### Dibware.EventDispatcher.UI

Lets add a new button to the main form and name it `HelloWorldButton` and give it the text of "Hello World!". Now create a button click handler for the new button. In the handler lets use the `ApplicationEventDispatcher` to raise a new event called `HelloWorldShouted`.

    private void HelloWorldButton_Click(object sender, EventArgs e)
    {
        ApplicationEventDispatcher.Dispatch(new HelloWorldShouted());
    }
        
As we haven't got an event called `HelloWorldShouted`, we had better create one in the `Events` folder.

    internal class HelloWorldShouted : IApplicationEvent
    {
        public string Message
        {
            get { return "Hello, you event driven world, you!"; }
        }
    }

Now we need to add a handler method to the main Form controller and wire it up and un-wire it in the `v` and `UnwireApplicationEventHandlers` methods respectively.

    protected override void UnwireApplicationEventHandlers()
    {
        ApplicationEventDispatcher.RemoveListener<HelloWorldShouted>(HandleHelloWorld);
        ApplicationEventDispatcher.RemoveListener<ProcessStarted>(HandleProcessStarted);
    }

    protected override sealed void WireUpApplicationEventHandlers()
    {
        ApplicationEventDispatcher.AddListener<HelloWorldShouted>(HandleHelloWorld);
        ApplicationEventDispatcher.AddListener<ProcessStarted>(HandleProcessStarted);
    }

Put a break point in the 'HelloWorldButton_Click' method of the `MainForm`, run the application and click the `HelloWorldButton` a few times. you will need to close the message box each time of course! You will likely notice that a new `HelloWorldShouted` event object is created each time. Each time this event is instantiated, it is exactly the same as the last, so this may be a good candidate for using our `ApplicationEventPool`. Lets have a look at implementing this.

In the `MainProcess` we will create a new private field of interface type `IApplicationEventPool` and initialise it in the constructor as the concrete class `ApplicationEventPool`. we will also pass a reference to the `MainFormController` via it's constructor as the interface `IApplicationEventPool`. This will allow us later to swap out the `ApplicationEventPool` for another should we want to for testing or enhancement purposes.

    internal class MainProcess : ApplicationEventHandlingBase
    {
        private readonly MainFormController _mainFormController;
        private readonly IApplicationEventPool _applicationEventPool;

        public MainProcess(IApplicationEventDispatcher applicationEventDispatcher)
            : base(applicationEventDispatcher)
        {
            _applicationEventPool = new ApplicationEventPool();
            _mainFormController = new MainFormController(applicationEventDispatcher, _applicationEventPool);
        }

In the `MainFormController` we will cache the `IApplicationEventPool` before passing the reference on once more to the `MainForm`. 

    private readonly IApplicationEventPool _applicationEventPool;
    private MainForm _mainForm;

    public MainFormController(IApplicationEventDispatcher applicationEventDispatcher, 
        IApplicationEventPool applicationEventPool)
        : base(applicationEventDispatcher)
    {

        _applicationEventPool = applicationEventPool;
        _mainForm = new MainForm(applicationEventDispatcher, applicationEventPool);

        WireUpApplicationEventHandlers();
    }

Finally in the `MainForm` we will cache the `IApplicationEventPool` once more so it can be access throughout the form code.

    private readonly IApplicationEventPool _applicationEventPool;

    public MainForm(IApplicationEventDispatcher applicationEventDispatcher, 
        IApplicationEventPool applicationEventPool)
        : base(applicationEventDispatcher)
    {
        InitializeComponent();
    }

Now we have a reference to the `ApplicationEventPool` in the `MainForm` lets modify the code in the `HelloWorldButton_Click` button handler method.
    
    private void HelloWorldButton_Click(object sender, EventArgs e)
    {
        HelloWorldShouted @event;

        var eventAlreadyCached = _applicationEventPool.TryGet(out @event);
        if (!eventAlreadyCached)
        {
            @event = new HelloWorldShouted();
            _applicationEventPool.TryAdd(@event);
        }

        ApplicationEventDispatcher.Dispatch(@event);
    }
    
So now we declare an event but we wont instantiate it yet. We will try and get this event from the `ApplicationEventPool` and if it exists we will pass that to teh event dispatcher, but if one does not exist then we will create one and pass that instead.

### Clean-up

We do have an small clean-up task to do in the `MainProcess` where the `ApplicationEventPool` was instantiated. We must clear the pool in the `Dispose(bool)` method.

    protected override void Dispose(bool disposing)
    {
        if (Disposed) return;

        if (disposing)
        {
            // Free other managed objects that implement IDisposable only
            _mainFormController.Dispose();
            _applicationEventPool.Clear();
            UnwireApplicationEventHandlers();
        }

        // release any unmanaged objects
        // set the object references to null

        Disposed = true;
    }

### Summary
So now we can reuse any common events through the lifetime of the application. However we are not out of the woods yet. Why? Well, this event pool is great for simple events, that carry no data or immutable data that will be the same for each time the event is dispatched but what if our events are mutable or immutable after construction? We can't trust that they will be in the right state when we dispatch them so two class could dispatch the event and require different data within the event. so how will we handle that? Well we will look at handling them in the next part, part 4.

## Disclaimer - Memory Usage
I am not an expert on Garbage Collection, so please be aware that pooling may not be the best solution for a memory problem using this Event Dispatcher. Pooling may lead to objects that should be short lived and would normally live in Generation 0 and get reclaimed quickly, being moved to Generation 1 or 2 and maybe not being reclaimed until the process ends. You can use `PerfMon.exe` or other profiling tools to help identify memory issues, and make an informed decision on how to combat them.

For further information on the Garbage Collector and [Garbage Collector Generations](https://msdn.microsoft.com/en-us/library/ee787088(v=vs.110).aspx#generations) please see [Fundamentals of Garbage Collection](https://msdn.microsoft.com/en-us/library/ee787088(v=vs.110).aspx)