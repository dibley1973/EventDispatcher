using Dibware.EventDispatcher.Core.Contracts;

namespace Dibware.EventDispatcher.Core.Tests.Fakes.Events
{
    public class EventWithSimpleData : IApplicationEvent
    {
        private readonly string _message;

        public EventWithSimpleData(string message)
        {
            _message = message;
        }

        public string Message
        {
            get { return _message; }
        }

        public override int GetHashCode()
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
    }
}
