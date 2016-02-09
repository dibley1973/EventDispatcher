using Dibware.EventDispatcher.Core.Contracts;
using Dibware.EventDispatcher.UI.Base;

namespace Dibware.EventDispatcher.UI
{
    internal class MainProcess : ApplicationEventHandlingBase
    {
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
    }
}