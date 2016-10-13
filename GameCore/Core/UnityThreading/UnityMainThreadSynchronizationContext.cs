using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GameCore.Core.UnityThreading
{
    public class UnityMainThreadSynchronizationContext : SynchronizationContext
    {
        public override void Post(SendOrPostCallback d, object state)
        {
            UnityMainThread.QueueUserWorkItem((innerState) =>
            {
                if (d != null)
                {
                    d(innerState);
                }
            }, state);
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            if(d != null)
                d(state);
        }
    }
}
