# Centralised Event Dispatcher in C# - Part 4
This is part four of a series of posts where I am going to share with you the solution I have used in a recent project for a centralised event dispatcher. All of the source code will be available in my public GitHub repositories. The concept I have tried to realise is a central class that can raise events for any listener to subscribe to. 

In [part one](http://www.duanewingett.info/2016/02/10/CentralisedEventDispatcherInCPart1.aspx) we created the event dispatcher class library.
In [part two](http://www.duanewingett.info/2016/02/11/CentralisedEventDispatcherInCPart2.aspx) we consumed it from a windows forms application. 
In [part three](http://www.duanewingett.info/2016/02/11/CentralisedEventDispatcherInCPart3.aspx) we look at adding pooling for commonly raised events and implement an 'ApplicationEventPool'.
In this part we will look at applying a restriction on what events can be safely pooled.

## Assumptions
It is assumed that the reader of this article already has a good understanding of coding with C#, can create projects and solutions, classes and Windows Forms, and can add references to a project and import references into a class. It is also assumed the reader understands basic inheritance and interface implementation.

So in the previous  part we added a class which will act as a pool for our various events which is great for immutable events whose data remains the same throughout their life time, but not suitable for events where the data could change during the life time or where there may be more than one active event in of the same type in the system but carrying different data.

Lets set up a scenario like this and then look at the options we have available.

### Dibware.EventDispatcher.UI

Lets add a couple of forms to the Forms folder of the UI project. Both will have a label, a text box and a button named `DisplayMessageLabel`, `CreateMessageTextBox` and `SendMessageButton` respectively. The two forms will be named `FormA` and `FormB`

Both forms will inherit from `` and both will require a private field to hold an instance of the `IApplicationEventPool`, for example:

    private readonly IApplicationEventPool _applicationEventPool;

    public FormA(
        IApplicationEventDispatcher applicationEventDispatcher,
        IApplicationEventPool applicationEventPool)
        : base(applicationEventDispatcher)
    {
        InitializeComponent();

        _applicationEventPool = applicationEventPool;
    }

Both forms will publish an event which contains message text and a reference to the sender and both forms will be capable of handling these messages. The reference to the sender is simply so that when a form tries to handle a message it sent it can ignore it. So first lets add a button handler to the form to publish the event.

    private void SendMessageButton_Click(object sender, System.EventArgs e)
    {
        var @event = new MessageEvent(this, CreateMessageTextBox.Text);
        ApplicationEventDispatcher.Dispatch(@event);
    }
        
And now lets create a new event to carry the data we want to send.

    internal class MessageEvent : IApplicationEvent
    {
        private readonly object _sender;
        private readonly string _text;

        public MessageEvent(object sender, string text)
        {
            _sender = sender;
            _text = text;
        }

        public object Sender
        {
            get { return _sender; }
        }

        public string Text
        {
            get { return _text; }
        }
    }
    
HINT: If you have the Resharper tool installed it is trivial to create the an immutable class like this from just the line `var @event = new MessageEvent(sender, CreateMessageTextBox.Text);` See my separate blog post [Using ReSharper to Create an Immutable Class](http://www.duanewingett.info/2016/02/12/UsingReSharperToolToCreateAnImmutableClass.aspx).

Now the event has been created we can switch back to the code behind the two new forms and wire in the handlers for the new event.

    protected override void UnwireApplicationEventHandlers()
    {
        ApplicationEventDispatcher.RemoveListener<MessageEvent>(HandleMessageEvent);
    }

    protected override sealed void WireUpApplicationEventHandlers()
    {
        ApplicationEventDispatcher.AddListener<MessageEvent>(HandleMessageEvent);
    }
    
Ensure you call the `WireUpApplicationEventHandlers()` method from the constructor after `_applicationEventPool = applicationEventPool;` and call the `UnwireApplicationEventHandlers()` before disposing of the `components` method in the dispose(bool) for the form.

Now we can look at displaying the message text so lest create a simple method to handle this.

    private void DisplayMessageText(string text)
    {
        DisplayMessageLabel.Text = text;
    }

We can also create the `HandleMessageEvent` method stub in each form.

    private void HandleMessageEvent(MessageEvent @event)
    {
        throw new System.NotImplementedException();
    }
    
But we can not call this directly from the `HandleMessageEvent` method when we create it as we cannot guarantee that any event we are listening to will be raised on the UI thread. If they are not and we try to access the UI component from a non-UI thread, then we may experience a cross-thread exception. So before we can pass information from the event thread to the UI updating method we must first check if an invoke is required. For this I will use an extension method `CallOrInvokeAsRequired` which is based strongly on the `SynchronizeInvokeExtensions.cs` class courtesy of [Michael Scrivo](https://github.com/mscrivo) which can be found [here](https://github.com/mscrivo/ootd/blob/master/OutlookDesktop/Utility/SynchronizeInvokeExtensions.cs)

    /// <summary>
    /// Invokes the specified action if InvokeRequired, or calls action directly if not
    /// Based upon Ref:
    /// https://github.com/mscrivo/ootd/blob/master/OutlookDesktop/Utility/SynchronizeInvokeExtensions.cs
    /// </summary>
    public static class SynchronizeInvokeExtensions
    {
        public static void CallOrInvokeAsRequired<T>(this T instance, Action<T> action) where T : ISynchronizeInvoke
        {
            if (instance.InvokeRequired)
            {
                instance.Invoke(action, new object[] { instance });
            }
            else
            {
                action(instance);
            }
        }
    }

So we can now update `HandleMessageEvent` to look call through to the `DisplayMessageText` method this, remembering to check if the sender is not the current form before attempting to display the message. We don't need to display a message which we sent!

    private void HandleMessageEvent(MessageEvent @event)
    {
        if (@event.Sender == this) return;

        this.CallOrInvokeAsRequired(form => form.DisplayMessageText(@event.Text));
    }

The next task is to return to the `MainForm` to add a new button to it to spawn these two new forms. So let us create a new button named `MessagingButton` with the text "Messaging". Lets add a `Click` handler to the form for that button and in a quick and dirty way create and instance of both `` and `` and display them.

    private void MessagingButton_Click(object sender, EventArgs e)
    {
        var formA = new FormA(ApplicationEventDispatcher, _applicationEventPool);
        var formB = new FormB(ApplicationEventDispatcher, _applicationEventPool);

        formA.Show();
        formB.Show();
    }

Spin up the application, click the "Messaging" button, arrange `FormA` and `FormB` instances next to each other and type a message in the TextBoxes and send them to the other form. Note you can actually spawn multiple instances of each form if you keep clicking the "Messaging" button on the main form. All instances of the form except the sender will display the message.

So in the last part, [Part 3](http://www.duanewingett.info/2016/02/11/CentralisedEventDispatcherInCPart3.aspx), we implemented an ApplicationEventPool to save on resources, so ideally it would be nice to use this. we even have an reference of the `ApplicationEventPool` available in both `FormA` and `FormB` as `_applicationEventPool`. The problem we have is the `ApplicationEventPool` is great for events which always have the same data, or no data, but it does not really work for events where the data is mutable or will need to differ each time the event is dispatched. It also will not work where there are many events of the same type being used but with different data, as once the first event is cached in the pool, because the dictionary key is based upon the type we can opnly add one event of a given type to the pool. What we need is a different key that can:
A) Differentiate the event by type, but also 
B) Differentiate between events of same type by their event data.

First we need to create a test the proves the failure of our current system. To do this we need a test project and some unit tests.

### Dibware.EventDispatcher.Core.Tests
PLEASE NOTE: This test project is available in the GutHub repository, [here](https://github.com/dibley1973/EventDispatcher/tree/master/Dibware.EventDispatcher.Core.Tests)

In a folder named `Events` which is inside a folder called `Fakes` we will create a simple event with some basic data.

    public class EventWithSimpleData : IApplicationEvent
    {
        private readonly string _message;

        public EventWithSimpleData(string message)
        {
            _message = message;
        }

        public string Message
        {
            get { return _message; }
        }
    }

In a folder named `Tests` we will create a test class named `ApplicationEventPoolTests`. In this class we will write a couple of tests that prove we have an issue. The first test which check of `TryAdd` fails to add if two events of the same type and same data are added. The test passes as teh expected outcome is `false`.

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
    
The next test will try to add two events of the same type but with different data. In an ideal world this would be allowed to happen. In our current world it doesn't and the test fails as the expected outcome is `true`, but the actual outcome is `false`.

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

Now we can look to discover and implement a solution to do this as we have a tests that will corroborate if teh new solution works. One option is to create a unique HashCode based upon a combination of the type of the event combined with  the data the event is carrying.

Lets change the interface which our events use to force implementation of a couple of new members. We are going to make our `IApplicationEvent` interface implement the `IEqualityComparer<IApplicationEvent>` interface like so.

    public interface IApplicationEvent : IEqualityComparer<IApplicationEvent> {}

The `IEqualityComparer<T>` interface will bring along the `Equals(T, T)` and `GetHashCode(obj)` members

public interface IEqualityComparer<T> {
    bool Equals(T x, T y);
    int GetHashCode(T obj);
}
   
T.B.C....  
 