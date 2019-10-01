using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Runtime.Async.UnityTasks
{
    internal class UnityUpdateTaskScheduler : TaskScheduler
    {
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return UnityMainThreadExecutionTaskQueue.UpdateTask.ToArray();
        }

        protected override void QueueTask(Task task)
        {
            UnityMainThreadExecutionTaskQueue.UpdateTask.Enqueue(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }
    }

    internal class UnityLateUpdateTaskScheduler : TaskScheduler
    {
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return UnityMainThreadExecutionTaskQueue.LateUpdateTask.ToArray();
        }

        protected override void QueueTask(Task task)
        {
            UnityMainThreadExecutionTaskQueue.LateUpdateTask.Enqueue(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }
    }

    internal class UnityFixedUpdateTaskScheduler : TaskScheduler
    {
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return UnityMainThreadExecutionTaskQueue.FixedUpdateTask.ToArray();
        }

        protected override void QueueTask(Task task)
        {
            UnityMainThreadExecutionTaskQueue.FixedUpdateTask.Enqueue(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }
    }
}