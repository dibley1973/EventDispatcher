using Dibware.EventDispatcher.Core.Contracts;
using Dibware.EventDispatcher.UI.Events;
using Dibware.EventDispatcher.UI.Forms.Base;
using System;

namespace Dibware.EventDispatcher.UI.Forms
{
    public partial class MainForm : ApplicationEventHandlingFormBase
    {
        public MainForm(IApplicationEventDispatcher applicationEventDispatcher)
            : base(applicationEventDispatcher)
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Hide();
            ApplicationEventDispatcher.Dispatch(new ProcessExiting());
        }

        private void HelloWorldButton_Click(object sender, EventArgs e)
        {
            HelloWorldShouted @event = new HelloWorldShouted();

            ApplicationEventDispatcher.Dispatch(@event);
        }

        private void MessagingButton_Click(object sender, EventArgs e)
        {
            var formA = new FormA(ApplicationEventDispatcher);
            var formB = new FormB(ApplicationEventDispatcher);

            formA.Show();
            formB.Show();
        }
    }
}