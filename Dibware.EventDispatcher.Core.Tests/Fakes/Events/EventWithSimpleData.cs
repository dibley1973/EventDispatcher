using Dibware.EventDispatcher.Core.Contracts;

namespace Dibware.EventDispatcher.Core.Tests.Fakes.Events
{
    public class EventWithSimpleData : IPoolableApplicationEvent
    {
        private readonly string _message;
        private readonly int _key;

        public EventWithSimpleData(string message)
        {
            _message = message;
            _key = GetHashCode();
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

        //public bool Equals(IApplicationEvent x, IApplicationEvent y)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public int GetHashCode(IApplicationEvent obj)
        //{
        //    // Ref: http://stackoverflow.com/a/263416/254215
        //    unchecked // Overflow is fine, just wrap
        //    {
        //        int hash = 17;
        //        hash = hash * 23 + Message.GetHashCode();
        //        return hash;
        //    }
        //}

        //public bool Equals(IApplicationEvent other)
        //{
        //    throw new System.NotImplementedException();
        //}

        public int Key
        {
            get { return _key; }
        }
    }
}
