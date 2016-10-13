using System.Collections.Generic;
using System.Threading;

namespace GameCore.Core.UnityThreading
{
    public static class UnityThreadPool
    {
        public static int MaxWorkerCount = 8;
        private static List<UnityThreadWorker> _workers = new List<UnityThreadWorker>();
        public static void QueueUserWorkItem(WaitCallback callback, object state)
        {
            if (MaxWorkerCount <= 0)
            {
                MaxWorkerCount = 1;
            }
            lock (_workers)
            {
                _workers.RemoveAll(e => !e.IsRunning);

                var currentWorker = default(UnityThreadWorker);
                if (_workers.Count == 0 || _workers.Count < MaxWorkerCount)
                {
                    currentWorker = new UnityThreadWorker();
                    _workers.Add(currentWorker);
                }
                else
                {
                    var queueSizeMin = int.MaxValue;
                    for (int i = 0; i < _workers.Count; i++)
                    {
                        var worker = _workers[i];
                        if (worker.QueueSize < queueSizeMin)
                        {
                            queueSizeMin = worker.QueueSize;
                            currentWorker = worker;
                        }
                    }
                }
                currentWorker.QueueUserWorkItem(callback, state);
            }
            
        }
    }
}

