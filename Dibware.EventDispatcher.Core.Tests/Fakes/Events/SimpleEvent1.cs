using Dibware.EventDispatcher.Core.Contracts;

namespace Dibware.EventDispatcher.Core.Tests.Fakes.Events
{
    internal class SimpleEvent1 : IPoolableApplicationEvent
    {
        private readonly int _hashCode;

        public SimpleEvent1()
        {
            _hashCode = GetHashCode();
        }

        private new int GetHashCode()
        {
            // Ref: http://stackoverflow.com/a/263416/254215
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + GetType().ToString().GetHashCode();
                return hash;
            }
        }

        public int HashCode
        {
            get { return _hashCode; }
        }
    }
}
