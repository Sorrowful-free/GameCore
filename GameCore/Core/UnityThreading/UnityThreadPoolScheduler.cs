using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameCore.Core.UnityThreading
{
    public class UnityThreadPoolScheduler : TaskScheduler
    {
        private static UnityThreadPoolScheduler _instance;
        public static UnityThreadPoolScheduler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UnityThreadPoolScheduler();
                }
                return _instance;
            }
        }
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            throw new System.NotImplementedException();
        }

        protected override void QueueTask(Task task)
        {
            UnityThreadPool.QueueUserWorkItem(TaskExecuterCallback, task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (taskWasPreviouslyQueued && !TryDequeue(task))
                return false;

            return TryExecuteTask(task);
        }
    }
}
