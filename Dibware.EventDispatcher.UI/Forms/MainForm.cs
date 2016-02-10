using Dibware.EventDispatcher.Core.Contracts;
using Dibware.EventDispatcher.UI.Forms.Base;
using System;
using Dibware.EventDispatcher.UI.Events;

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
            ApplicationEventDispatcher.Dispatch(new ProcessExiting());
        }
    }
}