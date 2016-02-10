# Centralised Event Dispatcher in C# - Part 2
This is the second part in a series of posts where I am going to share with you the solution I have used in a recent project for a centralised event dispatcher. All of the source code will be available in my public GitHub repositories. The concept I have tried to realise is a central class that can raise events for any listener to subscribe to. In [part one](http://www.duanewingett.info/2016/02/10/CentralisedEventDispatcherInCPart1.aspx) we created the event dispatcher class library, and in this part we are going to consume it from a windows forms application.

## Assumptions
It is assumed that the reader of this article already has a good understanding of coding with C#, can create projects and solutions, classes and Windows Forms, and can add references to a project and import references into a class. It is also assumed the reader understands basic inheritance and interface implementation.

### Dibware.EventDispatcher.UI
    
So now that is the actual event dispatcher complete so lets move to the windows forms project and write some code to use the event dispatcher. First we need to add a reference to the `Dibware.EventDispatcher.Core` project in the `Dibware.EventDispatcher.UI` project.

In the `Program.cs` we need a variable to hold a reference to an instance of the ApplicationEventDispatcher class which we will instantiate inside the `try` block of a `try-catch-finally` construct, ensuring the dispatcher is disposed in the `finally` block.

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ApplicationEventDispatcher applicationEventDispatcher = null;

            try
            {
                applicationEventDispatcher = new ApplicationEventDispatcher();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error!");
            }
            finally
            {
                if (applicationEventDispatcher != null) applicationEventDispatcher.Dispose();
            }
        }

Now assuming that we may have more than one class that will want to subscribe to the application events lets now create a base class that all subscribing classes will inherit from. I'm going to name it `ApplicationEventHandlingBase` and place it in a folder called `base`. This base class will implement `IDisposable` as we are going to want to clear up any event handlers when we finish with the objects that implement this class.

    internal abstract class ApplicationEventHandlingBase : IDisposable
    {
        protected bool Disposed { get; set; }

        protected abstract void Dispose(bool disposing);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

We are going to need a private field to hold a reference to an instance of the `ApplicationEventDispatcher` and we will want to initialise this from the constructor. We can then encapsulate the field with a protected property so all inherited classes have access to it. Finally we are going to define some methods for wiring and un-wiring events which must be implemented. That gives us this...

    internal abstract class ApplicationEventHandlingBase : IDisposable
    {
        private readonly IApplicationEventDispatcher _applicationEventDispatcher;

        protected ApplicationEventHandlingBase(IApplicationEventDispatcher applicationEventDispatcher)
        {
            if(applicationEventDispatcher == null) throw new ArgumentNullException("applicationEventDispatcher");

            _applicationEventDispatcher = applicationEventDispatcher;
        }

        protected IApplicationEventDispatcher ApplicationEventDispatcher
        {
            get { return _applicationEventDispatcher; }
        }

        protected bool Disposed { get; set; }

        protected abstract void Dispose(bool disposing);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void UnwireApplicationEventHandlers();
        protected abstract void WireUpApplicationEventHandlers();
    }

Now we have some base implementation for classes that will handle the application events, lets create the main process class, which will inherit from the base event handling class. Implementing the inherited and abstract members along with the Dispose pattern gives us this.

        public MainProcess(IApplicationEventDispatcher applicationEventDispatcher) 
            : base(applicationEventDispatcher)
        {
        }

        ~MainProcess()
        {
            Dispose(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (Disposed) return;

            if (disposing)
            {
                // Free other managed objects that implement IDisposable only

            }

            // release any unmanaged objects
            // set the object references to null

            Disposed = true;
        }

        protected override void UnwireApplicationEventHandlers()
        {

        }

        protected override void WireUpApplicationEventHandlers()
        {

        }

We can now construct this in the `Main` method of `program.cs` using the reference to the `ApplicationEventDispatcher` and make the call to run the application, not forgetting to add clean up for it in the `finally block`.

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ApplicationEventDispatcher applicationEventDispatcher = null;
            MainProcess mainProcess = null;
            try
            {
                applicationEventDispatcher = new ApplicationEventDispatcher();
                mainProcess = new MainProcess(applicationEventDispatcher);

                Application.Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error!");
            }
            finally
            {
                if (mainProcess != null) mainProcess.Dispose();
                if (applicationEventDispatcher != null) applicationEventDispatcher.Dispose();
            }
        }

So lets crack on and create controller class that will be responsible for opening the main form for the application. I'm going to name this `MainFormController` and as it will be responding to events it will inherit from the `ApplicationEventHandlingBase` class and we will implement the dispose pattern again, although we will leave it without any actual clean-up implementation for the moment. I'll add this class to a folder names `FormControllers`

    internal class MainFormController : ApplicationEventHandlingBase
    {
        public MainFormController(IApplicationEventDispatcher applicationEventDispatcher) 
            : base(applicationEventDispatcher)
        {

        }

        ~MainFormController()
        {
            Dispose(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (Disposed) return;

            if (disposing)
            {
                // free other managed objects that implement IDisposable only

            }

            // release any unmanaged objects
            // set the object references to null

            Disposed = true;
        }

        protected override void UnwireApplicationEventHandlers()
        {
            throw new System.NotImplementedException();
        }

        protected override void WireUpApplicationEventHandlers()
        {
            throw new System.NotImplementedException();
        }
    }

As this responsibility of this class will be to handle all actions and behaviour for the main form then we had better add  a for for it to work with, so let's create a form called `MainForm` and add it to a "Forms" folder. we are not to fussed about what it looks like for the moment, we just want something that the `MainFormController` can display when it needs to. Switching back to the `MainFormController` we need to add a private field to hold a reference to the form and lets instantiate it from the constructor. _Please note this article is not about good windows form application design so do not take my way of displaying forms as any kind of windows forms application gospel, its purely to show how you can hook into the EventDispatcher!_

So when do we want this form to display? Well ideally as soon as the main process has started, so lets switch back to the `MainProcess` class and create a new public method called `Start`, which we will call immediately after instantiating the `MainProcess` in the `Main` method of `program.cs`

    applicationEventDispatcher = new ApplicationEventDispatcher();
    mainProcess = new MainProcess(applicationEventDispatcher);
    mainProcess.Start();

The `Start` method will do nothing except raise a new `ProcessStarted` event, using our event dispatcher.

    public void Start()
    {
        ApplicationEventDispatcher.Dispatch(new ProcessStarted());
    }

Now as we haven't actually created a `ProcessStarted` we had better get on and do it, so lets create one in a new folder called `Events`. The `ProcessStarted` event class will implement the `IApplicationEvent` contract, which which the 'ApplicationEventDispatcher' states must be so. There is much else going on in this event, it does not have any data to go with it, but in time you may decide you want to add some, like maybe a `TimeStamp` which some class that will handle this event may wish to use. For the moment and for simplicity, lets just leave this class empty.

    internal class ProcessStarted : IApplicationEvent
    {
    }

So thats all very well but nothing is listening for this event yet, but we do have a class that wants to, the `MainFormController`. So lets create a private field for that controller in the `MainProcess` class and instantiate it in the constructor of `MainProcess` handing it a reference to the `ApplicationEventDispatcher`

    private MainFormController _mainFormController;

    public MainProcess(IApplicationEventDispatcher applicationEventDispatcher)
        : base(applicationEventDispatcher)
    {
        _mainFormController = new MainFormController(applicationEventDispatcher);
    }

We can now switch back to the `MainFormController`and wire up the handler for the `ProcessStarted` event  handler in the `WireUpApplicationEventHandlers` method...

    protected override sealed void WireUpApplicationEventHandlers()
    {
        ApplicationEventDispatcher.AddListener<ProcessStarted>(HandleProcessStarted);
    }

... and then create a method called `HandleProcessStarted` which will respond to the `ProcessStarted` event, which in this case will simply show the `_mainForm`.

    private void HandleProcessStarted(ProcessStarted @event)
    {
        _mainForm.Show();
    }

We will need to ensure `WireUpApplicationEventHandlers` is called from the constructor of the `MainFormController`, and may need to ensure that method as sealed to prevent a virtual member call compile error. if we run up the application then we should see the `MainForm` is displayed once the process is started. Our event dispatcher is working. What we have not done yet is any clear up of any resources, so lets take a look at that.

One the `MainForm` lets create a button which we will use to exit the application and add a `Click` handler for it in the code behind the form. We want this form to also be able to raise application events so it will need to be able to have a reference to the `ApplicationEventDispatcher`. So I am going to go against 'YAGNI' and create and create a base form class which will handle some of this for us. Within the `Forms` folder I am going to create a new folder named `Base`, and within that create a class named `ApplicationEventHandlingFormBase` which will inherit from `System.Windows.Forms.Form`. It will hold a private reference to the `ApplicationEventDispatcher` which will be passed in the constructor. You may notice we need a default parameterless constructor for the visual studio form designer. I have marked this with the `ObsoleteAttribute` to prevent the developer trying to call it! I have also added a protected property to allow all inherited classes access to the `ApplicationEventDispatcher` and two virtual methods for wiring and un-wiring application event handlers.

    public class ApplicationEventHandlingFormBase : Form
    {
        private readonly IApplicationEventDispatcher _applicationEventDispatcher;

        [Obsolete("Required by form designer", true)]
        protected ApplicationEventHandlingFormBase()
        {
        }

        protected ApplicationEventHandlingFormBase(IApplicationEventDispatcher applicationEventDispatcher)
        {
            if (applicationEventDispatcher == null) throw new ArgumentNullException("applicationEventDispatcher");

            _applicationEventDispatcher = applicationEventDispatcher;
        }

        protected IApplicationEventDispatcher ApplicationEventDispatcher
        {
            get { return _applicationEventDispatcher; }
        }

        protected virtual void UnwireApplicationEventHandlers()
        {
        }

        protected virtual void WireUpApplicationEventHandlers()
        {
        }
    }

We now need to make our `MainForm` inherit from `ApplicationEventHandlingFormBase` instead of `System.Windows.Forms.Form` and add a `IApplicationEventDispatcher` argument to the constructor...

    public partial class MainForm : ApplicationEventHandlingFormBase
    {
        public MainForm(IApplicationEventDispatcher applicationEventDispatcher)
            : base(applicationEventDispatcher)
        {
            InitializeComponent();
        }

...and pass a reference to the `ApplicationEventDispatcher` to it when it is constructed in the `MainFormController`
 
    _mainForm = new MainForm(applicationEventDispatcher);
 
We can now raise a new event `ProcessExiting` in the `ExitButton_Click` handler of the `MainForm`.
 
    private void ExitButton_Click(object sender, EventArgs e)
    {
        Hide();
        ApplicationEventDispatcher.Dispatch(new ProcessExiting());
    }
 
We have not yet created a class for that event, so lets do that back in the `Events` folder, ensuring it implements `IApplicationEvent`.
 
    internal class ProcessExiting : IApplicationEvent
    {
    }
 
 So now we need to decide what class which we already have may be interested in subscribing to that event. In my mind this is the `MainProcess` class, so in the `WireUpApplicationEventHandlers` we are going to hook up a listener to this event.
 
    protected override void WireUpApplicationEventHandlers()
    {
        ApplicationEventDispatcher.AddListener<ProcessExiting>(HandleProcessExiting);
    }
 
and create the handler method which exists the application.

    private void HandleProcessExiting(ProcessExiting @event)
    {
        Application.Exit();
    }

and ensure that the `WireUpApplicationEventHandlers` is called from the `Start` method.

    public void Start()
    {
        WireUpApplicationEventHandlers();
        ApplicationEventDispatcher.Dispatch(new ProcessStarted());
    }

We should probably dispose of `_mainForm` and set it to null in the `Dispose(bool)` method of the `MainFormController` too.
    
So lets run up the application and watch as the `MainForm` is displayed. Clicking the exit button closes the form and the application exits. So all is bright and rosy in this exciting event driven world, hey? Well not quite. If you place a break point in the `RemoveAllListeners` method of `ApplicationEventDispatcher` you will see this is called and the both of the event handlers are removed here. The issue we have is it's not really the responsibility for the `ApplicationEventDispatcher` to clear up after all of the classes which subscribe to its events. Its should still do a final check and clear up, but ideally each subscriber should clear up after its self.

#### Clean up

So lets look at the two classes which have subscribed to events from the `ApplicationEventDispatcher`. We have `MainProcess` and `MainFormController`. Now both of these classes implement `IDisposable`, so in the `disposing` code path of the `Dispose(bool)` method of both of these classes lets call `UnwireApplicationEventHandlers()`. Also in the `Dispose(bool)` method of  the `MainProcess` lets also ensure `Dispose` is called on the `_mainFormController`, so it can call through and un-wire it's handlers.

    protected override void Dispose(bool disposing)
    {
        if (Disposed) return;

        if (disposing)
        {
            // Free other managed objects that implement IDisposable only
            _mainFormController.Dispose();
            UnwireApplicationEventHandlers();
        }

        // release any unmanaged objects
        // set the object references to null

        Disposed = true;
    }

Now if we move to the `UnwireApplicationEventHandlers` method of each class we will call 

    ApplicationEventDispatcher.RemoveListener<ProcessStarted>(HandleProcessStarted);
            
... for the 'MainFormController' class and...
            
    ApplicationEventDispatcher.AddListener<ProcessExiting>(HandleProcessExiting);

... for the `MainProcess` class respectively.
 

Now if we run the application with a breakpoint in the `RemoveAllListeners` method of `ApplicationEventDispatcher` you will see there are no handlers registered. Bingo! All subscribers that we currently have are cleaning up after themselves. What good little children we have! And we can feel warm and fluffy as we know we are doing our bit to help the GarbageCollector.
    
In part three we will look at pooling events commonly used events, as spawning lots of new event objects could lead to problems in a memory concious system.

Full code available here at [My EventDispatcher GitHub repository](https://github.com/dibley1973/EventDispatcher)

[Part One](http://www.duanewingett.info/2016/02/10/CentralisedEventDispatcherInCPart1.aspx)