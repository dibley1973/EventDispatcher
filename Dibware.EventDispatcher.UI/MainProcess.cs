using Dibware.EventDispatcher.Core.Contracts;
using Dibware.EventDispatcher.UI.Base;
using Dibware.EventDispatcher.UI.Events;
using Dibware.EventDispatcher.UI.FormControllers;
using System.Windows.Forms;

namespace Dibware.EventDispatcher.UI
{
    internal class MainProcess : ApplicationEventHandlingBase
    {
        private MainFormController _mainFormController;

        public MainProcess(IApplicationEventDispatcher applicationEventDispatcher)
            : base(applicationEventDispatcher)
        {
            _mainFormController = new MainFormController(applicationEventDispatcher);
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
                _mainFormController.Dispose();
                UnwireApplicationEventHandlers();
            }

            // release any unmanaged objects
            // set the object references to null

            Disposed = true;
        }

        private void HandleProcessExiting(ProcessExiting @event)
        {
            Application.Exit();
        }

        public void Start()
        {
            WireUpApplicationEventHandlers();
            //ApplicationEventDispatcher.Dispatch(new ProcessStarted());
            ApplicationEventDispatcher.Dispatch(new ProcessStarted());
        }

        protected override void UnwireApplicationEventHandlers()
        {
            ApplicationEventDispatcher.RemoveListener<ProcessExiting>(HandleProcessExiting);
        }

        protected override void WireUpApplicationEventHandlers()
        {
            ApplicationEventDispatcher.AddListener<ProcessExiting>(HandleProcessExiting);
        }
    }
}