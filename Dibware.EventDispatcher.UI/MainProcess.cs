using Dibware.EventDispatcher.Core.Contracts;
using Dibware.EventDispatcher.UI.Base;
using Dibware.EventDispatcher.UI.Events;
using Dibware.EventDispatcher.UI.FormControllers;
using System.Windows.Forms;
using Dibware.EventDispatcher.Core;

namespace Dibware.EventDispatcher.UI
{
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
                _applicationEventPool.Clear();
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