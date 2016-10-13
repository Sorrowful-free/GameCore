using System.Threading;

namespace GameCore.Core.UnityThreading
{
    public class UnityThreadPoolSynchronizationContext : SynchronizationContext
    {
        public override void Post(SendOrPostCallback d, object state)
        {
            UnityThreadPool.QueueUserWorkItem((innerState) =>
            {
                if (d != null)
                    d(innerState);
            },state);
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            if (d != null)
            {
                d(state);
            }
        }
    }
}
