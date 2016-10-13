using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace GameCore.Core.UnityThreading
{
    public class UnityThreadWorker
    {
        public int QueueSize{get{return _queue.Count;}}
        
        public bool IsRunning { get; private set; }
        private readonly ConcurrentQueue<Tuple<WaitCallback,object>> _queue;
        private readonly int _stepDelay;
        private readonly int _maxWaitTime;
        
        public UnityThreadWorker(int stepDelay = 5,int maxWaitTime = 1500)
        {
            _queue = new ConcurrentQueue<Tuple<WaitCallback, object>>();
            _stepDelay = stepDelay;
            _maxWaitTime = maxWaitTime;
            IsRunning = true;
            ThreadPool.QueueUserWorkItem(WorkerQueueProcess);
        }

        public void QueueUserWorkItem(WaitCallback callback, object state)
        {
            _queue.Enqueue(new Tuple<WaitCallback, object>(callback,state));
        }

        private void WorkerQueueProcess(object state)
        {
            var stopWatch = new Stopwatch();
            while (IsRunning)
            {
                while (_queue.Count>0)
                {
                    stopWatch.Reset();
                    stopWatch.Start();
                    var callbackPair = default(Tuple<WaitCallback, object>);
                    if (_queue.TryDequeue(out callbackPair))
                    {
                        try
                        {
                            if(callbackPair.Item1 != null)
                                callbackPair.Item1(callbackPair.Item2);
                        }
                        catch (Exception exception)
                        {
                            var dispatcher = ExceptionDispatchInfo.Capture(exception);
                            UnityMainThread.QueueUserWorkItem((innerState) =>
                            {
                                var innerDispatcher = innerState as ExceptionDispatchInfo;
                                innerDispatcher.Throw();
                            },dispatcher);
                        }
                    }
                }
                Thread.Sleep(_stepDelay);
                if (stopWatch.ElapsedMilliseconds > _maxWaitTime)
                {
                    IsRunning = false;
                    stopWatch.Stop();
                }
            }
            
        }
    }
}
