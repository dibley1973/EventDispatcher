using System;
using System.ComponentModel;

namespace Dibware.EventDispatcher.UI.Extensions
{
    /// <summary>
    /// Extension method to allow accessing properties and fields in a worker thread without throwing:
    /// "Cross-thread operation not valid: Control 'MainForm' accessed from a thread other than the thread it was created on."
    /// 
    /// Invokes the specified action if InvokeRequired, or calls action directly if not
    /// Based upon Ref:
    /// https://github.com/mscrivo/ootd/blob/master/OutlookDesktop/Utility/SynchronizeInvokeExtensions.cs
    /// </summary>
    public static class SynchronizeInvokeExtensions
    {
        public static void CallOrInvokeAsRequired<T>(this T instance, Action<T> action) where T : ISynchronizeInvoke
        {
            if (instance.InvokeRequired)
            {
                instance.Invoke(action, new object[] { instance });
            }
            else
            {
                action(instance);
            }
        }
    }
}