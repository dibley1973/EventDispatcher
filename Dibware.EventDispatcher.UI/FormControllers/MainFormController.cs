using Dibware.EventDispatcher.Core.Contracts;
using Dibware.EventDispatcher.UI.Base;
using Dibware.EventDispatcher.UI.Events;
using Dibware.EventDispatcher.UI.Forms;

namespace Dibware.EventDispatcher.UI.FormControllers
{
    internal class MainFormController : ApplicationEventHandlingBase
    {
        private MainForm _mainForm;

        public MainFormController(IApplicationEventDispatcher applicationEventDispatcher)
            : base(applicationEventDispatcher)
        {
            _mainForm = new MainForm(applicationEventDispatcher);

            WireUpApplicationEventHandlers();
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

        private void HandleProcessStarted(ProcessStarted @event)
        {
            _mainForm.Show();
        }

        protected override void UnwireApplicationEventHandlers()
        {
            throw new System.NotImplementedException();
        }

        protected override sealed void WireUpApplicationEventHandlers()
        {
            ApplicationEventDispatcher.AddListener<ProcessStarted>(HandleProcessStarted);
        }
    }
}