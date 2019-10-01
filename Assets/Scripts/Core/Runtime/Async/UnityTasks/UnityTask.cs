using System.Threading.Tasks;

namespace Core.Runtime.Async.UnityTasks
{
    public static class UnityTask
    {
        public static TaskFactory UpdateTaskFactory = new TaskFactory(new UnityUpdateTaskScheduler());
        public static TaskFactory LateUpdateTaskFactory = new TaskFactory(new UnityLateUpdateTaskScheduler());
        public static TaskFactory FixedUpdateTaskFactory = new TaskFactory(new UnityFixedUpdateTaskScheduler());
    }

    public static class UnityTask<T>
    {
        public static TaskFactory<T> UpdateTaskFactory = new TaskFactory<T>(new UnityUpdateTaskScheduler());
        public static TaskFactory<T> LateUpdateTaskFactory = new TaskFactory<T>(new UnityLateUpdateTaskScheduler());
        public static TaskFactory<T> FixedUpdateTaskFactory = new TaskFactory<T>(new UnityFixedUpdateTaskScheduler());
    }
}