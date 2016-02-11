using Dibware.EventDispatcher.Core.Contracts;
using Dibware.EventDispatcher.UI.Forms.Base;
using System;
using Dibware.EventDispatcher.UI.Events;

namespace Dibware.EventDispatcher.UI.Forms
{
    public partial class MainForm : ApplicationEventHandlingFormBase
    {
        private readonly IApplicationEventPool _applicationEventPool;

        public MainForm(IApplicationEventDispatcher applicationEventDispatcher, 
            IApplicationEventPool applicationEventPool)
            : base(applicationEventDispatcher)
        {
            _applicationEventPool = applicationEventPool;
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Hide();
            ApplicationEventDispatcher.Dispatch(new ProcessExiting());
        }

        private void HelloWorldButton_Click(object sender, EventArgs e)
        {
            HelloWorldShouted @event;

            var eventAlreadyCached = _applicationEventPool.TryGet(out @event);
            if (!eventAlreadyCached)
            {
                @event = new HelloWorldShouted();
                _applicationEventPool.TryAdd(@event);
            }

            ApplicationEventDispatcher.Dispatch(@event);
        }
    }
}