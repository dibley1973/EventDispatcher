using Dibware.EventDispatcher.Core.Contracts;
using Dibware.EventDispatcher.UI.Events;
using Dibware.EventDispatcher.UI.Extensions;
using Dibware.EventDispatcher.UI.Forms.Base;

namespace Dibware.EventDispatcher.UI.Forms
{
    public partial class FormA : ApplicationEventHandlingFormBase
    {
        private readonly IApplicationEventPool _applicationEventPool;

        public FormA(
            IApplicationEventDispatcher applicationEventDispatcher,
            IApplicationEventPool applicationEventPool)
            : base(applicationEventDispatcher)
        {
            InitializeComponent();

            _applicationEventPool = applicationEventPool;
            WireUpApplicationEventHandlers();
        }

        private void DisplayMessageText(string text)
        {
            DisplayMessageLabel.Text = text;
        }

        private void SendMessageButton_Click(object sender, System.EventArgs e)
        {
            var @event = new MessageEvent(this, CreateMessageTextBox.Text);
            ApplicationEventDispatcher.Dispatch(@event);
        }

        private void HandleMessageEvent(MessageEvent @event)
        {
            if (@event.Sender == this) return;

            this.CallOrInvokeAsRequired(form => form.DisplayMessageText(@event.Text));
        }

        protected override void UnwireApplicationEventHandlers()
        {
            ApplicationEventDispatcher.RemoveListener<MessageEvent>(HandleMessageEvent);
        }

        protected override sealed void WireUpApplicationEventHandlers()
        {
            ApplicationEventDispatcher.AddListener<MessageEvent>(HandleMessageEvent);
        }
    }
}