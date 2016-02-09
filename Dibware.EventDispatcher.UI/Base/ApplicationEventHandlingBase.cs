using Dibware.EventDispatcher.Core.Contracts;
using System;

namespace Dibware.EventDispatcher.UI.Base
{
    internal abstract class ApplicationEventHandlingBase : IDisposable
    {
        private readonly IApplicationEventDispatcher _applicationEventDispatcher;

        protected ApplicationEventHandlingBase(IApplicationEventDispatcher applicationEventDispatcher)
        {
            if (applicationEventDispatcher == null) throw new ArgumentNullException("applicationEventDispatcher");

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
}