using Dibware.EventDispatcher.Core.Contracts;

namespace Dibware.EventDispatcher.UI.Events
{
    internal class MessageEvent : IApplicationEvent
    {
        private readonly object _sender;
        private readonly string _text;

        public MessageEvent(object sender, string text)
        {
            _sender = sender;
            _text = text;
        }

        public object Sender
        {
            get { return _sender; }
        }

        public string Text
        {
            get { return _text; }
        }

    }
}