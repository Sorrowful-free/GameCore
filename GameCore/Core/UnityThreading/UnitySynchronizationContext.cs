using System.Threading;

namespace GameCore.Core.UnityThreading
{
    public static class UnitySynchronizationContext
    {
        static UnitySynchronizationContext()
        {
            Default = new UnityThreadPoolSynchronizationContext();
            Unity = new UnityMainThreadSynchronizationContext();
        }

        public static readonly SynchronizationContext Unity;
        public static readonly SynchronizationContext Default;

        public static void MakeUnity()
        {
            SynchronizationContext.SetSynchronizationContext(Unity);
        }

        public static void MakeDefault()
        {
            SynchronizationContext.SetSynchronizationContext(Default);
        }
    }
}
