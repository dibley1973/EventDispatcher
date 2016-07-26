using Dibware.EventDispatcher.Core.Contracts;
using System;
using System.Windows.Forms;

namespace Dibware.EventDispatcher.UI.Forms.Base
{
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
}
