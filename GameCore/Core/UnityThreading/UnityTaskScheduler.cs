using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameCore.Core.UnityThreading
{
    class UnityTaskScheduler : TaskScheduler
    {
        private static UnityTaskScheduler _instance;
        public static UnityTaskScheduler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UnityTaskScheduler();
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
