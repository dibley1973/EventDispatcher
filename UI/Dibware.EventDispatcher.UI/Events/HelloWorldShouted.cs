using Dibware.EventDispatcher.Core.Contracts;

namespace Dibware.EventDispatcher.UI.Events
{
    internal class HelloWorldShouted : IApplicationEvent
    {
        private readonly string _message;
        private readonly int _hashCode;

        public HelloWorldShouted()
        {
            _message = "Hello, you event driven world, you!";
            _hashCode = GetHashCode();
        }

        public string Message
        {
            get { return _message; }
        }

        private new int GetHashCode()
        {
            // Ref: http://stackoverflow.com/a/263416/254215
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + GetType().ToString().GetHashCode();
                hash = hash * 23 + Message.GetHashCode();
                return hash;
            }
        }

        public int HashCode
        {
            get { return _hashCode; }
        }
    }
}