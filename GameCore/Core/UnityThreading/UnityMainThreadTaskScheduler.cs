using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameCore.Core.UnityThreading
{
    class UnityMainThreadTaskScheduler : TaskScheduler
    {
        private static UnityMainThreadTaskScheduler _instance;
        public static UnityMainThreadTaskScheduler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UnityMainThreadTaskScheduler();
                }
                return _instance;
            }
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            throw new NotImplementedException();
        }

        protected override void QueueTask(Task task)
        {
            UnityMainThread.QueueUserWorkItem(TaskExecuterCallback, task);
        }


        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (taskWasPreviouslyQueued && !TryDequeue(task))
                return false;

            return TryExecuteTask(task);
        }


    }
}
