# Centralised Event Dispatcher in C# - Part 1
In this series of posts I am going to share with you the solution I have used in a recent project for a centralised event dispatcher. All of the source code will be available in my public GitHub repositories. The concept I have tried to realise is a central class that can raise events for any listener to subscribe to. 

## Assumptions
It is assumed that the reader of this article already has a good understanding of coding with C#, can create projects and solutions, classes and Windows Forms, and can add references to a project and import references into a class. It is also assumed the reader understands basic inheritance and interface implementation.

## Solution
I have created a solution with two projects, one a windows forms application project which I have called `Dibware.EventDispatcher.UI` and the other is a class library called `Dibware.EventDispatcher.Core`. I will place all of the event dispatcher code in the class library so that if you like the solution then you can just pick the DLL up and start using it in your own projects without the clutter of the consuming code.

### Dibware.EventDispatcher.Core
First I want to define a contract in the class library that all event objects will adhere to. I will name this `IApplicationEvent` and it will be an empty public interface in a folder named `Contracts`.

    public interface IApplicationEvent { }

Any method that wants to handle any of the events that the dispatcher will publish will need to conform to a predefined method signature. This will be defined by the `ApplicationEventHandlerDelegate` delegate in the `Contracts` folder.
    
    public delegate void ApplicationEventHandlerDelegate<in TEvent>(TEvent @event) where TEvent : IApplicationEvent;
    
This delegate has a generic type parameter `TEvent` which can be contravariant and will take a single argument of the generic type (which will be the event object) but the type of the event object will be constrained to implement the `IApplicationEvent` interface.

In the same folder I will create a public interface contract which the event dispatcher will adhere to, named `IApplicationEventDispatcher`. This interface will have three members, one to add a listener to the dispatcher, one to remove a listener from the dispatcher, and one to dispatch an event. The listeners must adhere to the signature defined by the `ApplicationEventHandlerDelegate`. The `Dispatch` method will take an argument which adheres to the `IApplicationEvent` interface. The `IApplicationEventDispatcher` will also demand that IDisposable is implemented so that any resources can be cleared up properly.

    public interface IApplicationEventDispatcher : IDisposable
    {
        void AddListener<TEvent>(ApplicationEventHandlerDelegate<TEvent> handler) where TEvent : IApplicationEvent;
        void RemoveListener<TEvent>(ApplicationEventHandlerDelegate<TEvent> handler) where TEvent : IApplicationEvent;
        void Dispatch<TEvent>(TEvent @event) where TEvent : IApplicationEvent;
    }

Now we can create the public dispatcher class itself, and we will put this in the root of the class library assembly. We will call it `ApplicationEventDispatcher` and it will implement the `IApplicationEventDispatcher` interface and inherently `IDisposable`.

    public class ApplicationEventDispatcher : IApplicationEventDispatcher
    {
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void AddListener<TEvent>(ApplicationEventHandlerDelegate<TEvent> handler) where TEvent : IApplicationEvent
        {
            throw new System.NotImplementedException();
        }

        public void RemoveListener<TEvent>(ApplicationEventHandlerDelegate<TEvent> handler) where TEvent : IApplicationEvent
        {
            throw new System.NotImplementedException();
        }

        public void Dispatch<TEvent>(TEvent @event) where TEvent : IApplicationEvent
        {
            throw new System.NotImplementedException();
        }
    }

So lets correctly implement the Dispose pattern within the class.

        private bool _disposed;

        ~ApplicationEventDispatcher()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // free other managed objects that implement IDisposable only
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

The next task is to create a backing store for the event handlers. For this we will use a dictionary where the key is the type of the event which is to be handled and the value is the delegate that will handle the event. This dictionary will be initialised in the class constructor. We will also need to dispose of it any any delagates within it properly later. but for now we will just set the reference to null in the `Dispose(bool)` method just before setting `_disposed = true;`.

        private Dictionary<Type, Delegate> _applicationEventHandlers;

        public ApplicationEventDispatcher()
        {
            _applicationEventHandlers = new Dictionary<Type, Delegate>();
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // free other managed objects that implement IDisposable only
            }

            // release any unmanaged objects
            // set the object references to null

            _applicationEventHandlers = null;

            _disposed = true;
        }

We can now focus on adding listeners to the dispatcher by adding implementation into the empty `AddListener` method. The first task is to see if our dictionary store already has any delegates for the type of event handler we are adding. If we do then we get a reference to the delegates and combine the new one with them. If there id not one present then we add the handler into the dictionary using the event type as the key.

        public void AddListener<TEvent>(ApplicationEventHandlerDelegate<TEvent> handler)
            where TEvent : IApplicationEvent
        {
            Delegate @delegate;
            if (_applicationEventHandlers.TryGetValue(typeof(TEvent), out @delegate))
            {
                _applicationEventHandlers[typeof(TEvent)] = Delegate.Combine(@delegate, handler);
            }
            else
            {
                _applicationEventHandlers[typeof(TEvent)] = handler;
            }
        }

If what goes up must come down then what gets added must be given a fair crack of the whip to be removed. We can provide this by adding implementation into the empty `RemoveListener` method. Again the first task is to see if we have any evens in the dictionary for the type of the event. If we do then we can look to see if our handler is in the delegates, and if it  remove it from the delegate invocation list. If there are no more delegates in the invocation list then remove the dictionary entry altogether.

        public void RemoveListener<TEvent>(ApplicationEventHandlerDelegate<TEvent> handler)
            where TEvent : IApplicationEvent
        {
            Delegate @delegate;
            if (_applicationEventHandlers.TryGetValue(typeof(TEvent), out @delegate))
            {
                Delegate currentDel = Delegate.Remove(@delegate, handler);

                if (currentDel == null)
                {
                    _applicationEventHandlers.Remove(typeof(TEvent));
                }
                else
                {
                    _applicationEventHandlers[typeof(TEvent)] = currentDel;
                }
            }
        }

Now we have methods to add handlers to and remove them from our dispatcher lets provide some functionality to dispatch an event to any subscribed listeners. If the event passed is null then throw an exception straight away, otherwise use the type of the event to look for handlers of that event in the dictionary, if there are handlers for the event, then invoke them!

    public void Dispatch<TEvent>(TEvent @event) where TEvent : IApplicationEvent
    {
        if (@event == null)
        {
            throw new ArgumentNullException("event");
        }

        Delegate @delegate;
        if (_applicationEventHandlers.TryGetValue(typeof(TEvent), out @delegate))
        {
            ApplicationEventHandlerDelegate<TEvent> callback = @delegate as ApplicationEventHandlerDelegate<TEvent>;
            if (callback != null)
            {
                callback(@event);
            }
        }
    }

Theoretically all code that subscribes to the event dispatcher's events will detach their own handlers, but being aware that subscribing code may be not carry out this task we need to ensure all handlers are disconnected when this event dispatcher is disposed. For this we will add a `RemoveAllListeners` method which we will call from the `disposing` code path in `Dispose(bool)`.

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // free other managed objects that implement IDisposable only
            }

            // release any unmanaged objects
            // set the object references to null
            RemoveAllListeners();

            _applicationEventHandlers = null;

            _disposed = true;
        }

And this method will gather all of the handler types, iterating through them un-wiring all of the delegates in the invocation lists and finally when no delegates exist for the handler type, removes the dictionary entry. 

        private void RemoveAllListeners()
        {
            var handlerTypes = new Type[_applicationEventHandlers.Keys.Count];
            _applicationEventHandlers.Keys.CopyTo(handlerTypes, 0);

            foreach (Type handlerType in handlerTypes)
            {
                Delegate[] delegates = _applicationEventHandlers[handlerType].GetInvocationList();
                foreach (Delegate @delegate1 in delegates)
                {
                    var handlerToRemove = Delegate.Remove(_applicationEventHandlers[handlerType], @delegate1);
                    if (handlerToRemove == null)
                    {
                        _applicationEventHandlers.Remove(handlerType);
                    }
                    else
                    {
                        _applicationEventHandlers[handlerType] = handlerToRemove;
                    }
                }
            }
        }

That is it for part one, we have created the `ApplicationEventDispatcher`. In part two we will look at implementing it in a Windows Forms application.

Full code available here at [My EventDispatcher GitHub repository](https://github.com/dibley1973/EventDispatcher)

[Part 2] (http://www.duanewingett.info/2016/02/11/CentralisedEventDispatcherInCPart2.aspx)