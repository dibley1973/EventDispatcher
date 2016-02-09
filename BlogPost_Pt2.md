# Centralised Event Dispatcher in C# - Part 2
This is the second part in a series of posts where I am going to share with you the solution I have used in a recent project for a centralised event dispatcher. All of the source code will be available in my public GitHub repositories. The concept I have tried to realise is a central class that can raise events for any listener to subscribe to. In part one we created the event dispatcher class library, and in this part we are going to consume it from a windows forms application.

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
